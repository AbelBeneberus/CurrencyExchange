namespace CurrencyExchange.Api.Model
{
	public class CurrencyExchangeRequest
	{
		public Guid CorrelationId { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public decimal Amount { get; set; }
	}
}