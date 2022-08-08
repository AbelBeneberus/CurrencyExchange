using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Domain.Events
{
	public class CurrencyExchangeRateCreatedEvent : CurrencyExchangeRateBaseEvent
	{
		public CurrencyExchangeRateCreatedEvent(CurrencyExchangeRate exchangeRate)
		{
			Rate = exchangeRate;
		}
		public CurrencyExchangeRate Rate { get; }
	}
}
