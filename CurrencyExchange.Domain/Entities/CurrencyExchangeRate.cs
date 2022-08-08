namespace CurrencyExchange.Domain.Entities;

public class CurrencyExchangeRate : BaseEntityWithDomainEvent
{
	public string From { get; set; }
	public string To { get; set; }
	public double TimeStamp { get; set; }
	public DateTime Date { get; set; }
	public decimal Rate { get; set; }
	public DateTime ValidFrom { get; set; }
	public DateTime ValidToDate { get; set; }
	public virtual ICollection<CurrencyTradeTransaction> Trades { get; set; }
}

