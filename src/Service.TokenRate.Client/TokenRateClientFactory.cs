using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Service.TokenRate.Grpc;
using Service.Grpc;

namespace Service.TokenRate.Client
{
    [UsedImplicitly]
    public class TokenRateClientFactory : GrpcClientFactory
    {
        public TokenRateClientFactory(string grpcServiceUrl, ILogger logger) : base(grpcServiceUrl, logger)
        {
        }

        public IGrpcServiceProxy<ITokenRateService> GetTokenRateService() => CreateGrpcService<ITokenRateService>();
    }
}
