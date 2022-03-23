using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Core.Client.Models;
using Service.Core.Client.Services;
using Service.TokenRate.Grpc;
using Service.TokenRate.Grpc.Models;
using Service.TokenRate.Postgres;
using Service.TokenRate.Postgres.Models;

namespace Service.TokenRate.Services
{
	public class TokenRateService : ITokenRateService
	{
		private readonly ILogger<TokenRateService> _logger;
		private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
		private readonly ISystemClock _systemClock;

		public TokenRateService(ILogger<TokenRateService> logger, DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder, ISystemClock systemClock)
		{
			_logger = logger;
			_dbContextOptionsBuilder = dbContextOptionsBuilder;
			_systemClock = systemClock;
		}

		public async ValueTask<TokenRateGrpcResponse> GetTokenRate() => new TokenRateGrpcResponse
		{
			Value = (await GetEntity())?.Value
		};

		private async Task<TokenRateEntity> GetEntity()
		{
			TokenRateEntity entity = null;

			try
			{
				entity = await GetContext()
					.TokenRateEntities
					.FirstOrDefaultAsync();
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);
			}

			return entity;
		}

		public async ValueTask<CommonGrpcResponse> SetTokenRate(SetTokenRateGrpcRequest request)
		{
			DatabaseContext context = GetContext();
			DbSet<TokenRateEntity> entities = context.TokenRateEntities;
			TokenRateEntity existingEntity = await GetEntity();

			void FillEntity(TokenRateEntity rateEntity)
			{
				rateEntity.Value = request.Value;
				rateEntity.Date = _systemClock.Now;
			}

			try
			{
				if (existingEntity == null)
				{
					var newEntity = new TokenRateEntity();
					FillEntity(newEntity);
					await entities.AddAsync(newEntity);
				}
				else
				{
					FillEntity(existingEntity);
					entities.Update(existingEntity);
					await context.SaveChangesAsync();
				}
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, exception.Message);

				return CommonGrpcResponse.Fail;
			}

			return CommonGrpcResponse.Success;
		}

		private DatabaseContext GetContext() => DatabaseContext.Create(_dbContextOptionsBuilder);
	}
}