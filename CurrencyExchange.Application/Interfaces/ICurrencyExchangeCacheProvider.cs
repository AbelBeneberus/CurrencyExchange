using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Application.Interfaces
{
	public interface ICurrencyExchangeCacheProvider
	{
		Task<CurrencyExchangeRate?> GetCachedCurrencyExchangeAsync(string key);
		Task CacheCurrencyExchangeAsync(string key, CurrencyExchangeRate currencyExchangeRate, TimeSpan? timeSpan);
	}
}
