namespace Service.TokenRate.Postgres.Models
{
	public class TokenRateEntity
	{
		public int Id { get; set; }

		public DateTime Date { get; set; }

		public decimal Value { get; set; }
	}
}