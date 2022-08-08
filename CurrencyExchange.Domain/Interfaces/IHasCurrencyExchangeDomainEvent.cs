using CurrencyExchange.Domain.Events;

namespace CurrencyExchange.Domain.Interfaces;

public interface IHasCurrencyExchangeDomainEvent
{
	public List<CurrencyExchangeRateBaseEvent> Events { get; set; }
}