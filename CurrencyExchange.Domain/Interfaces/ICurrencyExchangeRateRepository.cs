using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Domain.Interfaces
{
	public interface ICurrencyExchangeRateRepository
	{
		Task<int> CreateCurrencyExchangeRateAsync(CurrencyExchangeRate exchangeRate,
			CancellationToken cancellationToken);
	}
}
