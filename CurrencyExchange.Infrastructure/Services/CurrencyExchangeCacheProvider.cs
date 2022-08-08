using System.Text.Json;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Infrastructure.Extensions;
using StackExchange.Redis;

namespace CurrencyExchange.Infrastructure.Services
{
	public class CurrencyExchangeCacheProvider : ICurrencyExchangeCacheProvider
	{
		private readonly IDatabaseAsync _redisDbAsync;
		public CurrencyExchangeCacheProvider(ConnectionMultiplexer redisConnection)
		{
			_redisDbAsync = redisConnection.GetDatabase();
		}
		public async Task<CurrencyExchangeRate?> GetCachedCurrencyExchangeAsync(string key)
		{
			var cachedCurrencyExchangeInfo = await _redisDbAsync.StringGetAsync(key);
			return cachedCurrencyExchangeInfo.HasValue
				? JsonSerializer.Deserialize<CurrencyExchangeRate>(cachedCurrencyExchangeInfo)
				: null;
		}

		public Task CacheCurrencyExchangeAsync(string key, CurrencyExchangeRate currencyExchangeRate, TimeSpan? timeSpan)
		{
			var stringifyRate = JsonSerializer.Serialize(currencyExchangeRate);

			return _redisDbAsync.StringSetAsync(key.ToRedisKey(), stringifyRate.ToRedisValue(), timeSpan);
		}
	}

}
