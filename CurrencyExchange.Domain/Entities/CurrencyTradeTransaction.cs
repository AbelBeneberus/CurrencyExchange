namespace CurrencyExchange.Domain.Entities
{
	public class CurrencyTradeTransaction : BaseEntityWithDomainEvent
	{
		public Guid CorrelationId { get; set; }
		public string UserId { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public decimal AmountToBeConverted { get; set; }
		public decimal ConvertedAmount { get; set; }
		public double TimeStamp { get; set; }
		public DateTime TransactionDate { get; set; }
		public int UsedRateId { get; set; }
		public virtual CurrencyExchangeRate UsedRate { get; set; }

	}
}
