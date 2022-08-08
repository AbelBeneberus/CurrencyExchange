namespace CurrencyExchange.Domain.Events;

public abstract class CurrencyExchangeRateBaseEvent
{
	public bool IsPublished { get; set; }
	public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.Now;
}