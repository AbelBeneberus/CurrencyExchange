using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Domain.Interfaces
{
	public interface ITradingRepository
	{
		Task CreateCurrencyExchangeTransaction(CurrencyTradeTransaction transaction,
			CancellationToken cancellationToken);
	}
}
