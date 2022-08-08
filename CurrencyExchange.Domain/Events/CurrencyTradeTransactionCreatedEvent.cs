using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Domain.Events
{
	public class CurrencyTradeTransactionCreatedEvent : CurrencyExchangeRateBaseEvent
	{
		public CurrencyTradeTransactionCreatedEvent(CurrencyTradeTransaction tradeTransaction)
		{
			CurrencyTradeTransaction = tradeTransaction;
		}

		public CurrencyTradeTransaction CurrencyTradeTransaction { get; }
	}
}
