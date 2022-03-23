using Microsoft.EntityFrameworkCore;
using MyJetWallet.Sdk.Postgres;
using MyJetWallet.Sdk.Service;
using Service.TokenRate.Postgres.Models;

namespace Service.TokenRate.Postgres
{
	public class DatabaseContext : MyDbContext
	{
		public const string Schema = "education";
		private const string TokenRateTableName = "token_rate";

		public DatabaseContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<TokenRateEntity> TokenRateEntities { get; set; }

		public static DatabaseContext Create(DbContextOptionsBuilder<DatabaseContext> options)
		{
			MyTelemetry.StartActivity($"Database context {Schema}")?.AddTag("db-schema", Schema);

			return new DatabaseContext(options.Options);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema(Schema);

			SetUserInfoEntityEntry(modelBuilder);

			base.OnModelCreating(modelBuilder);
		}

		private static void SetUserInfoEntityEntry(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TokenRateEntity>().ToTable(TokenRateTableName);

			modelBuilder.Entity<TokenRateEntity>().Property(e => e.Date).ValueGeneratedOnAdd().IsRequired();
			modelBuilder.Entity<TokenRateEntity>().Property(e => e.Value).IsRequired();
			modelBuilder.Entity<TokenRateEntity>().Property(e => e.Id).ValueGeneratedOnAdd();
			
			modelBuilder.Entity<TokenRateEntity>().HasKey(e => e.Id);
		}
	}
}