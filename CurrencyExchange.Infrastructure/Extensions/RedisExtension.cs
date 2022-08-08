using StackExchange.Redis;

namespace CurrencyExchange.Infrastructure.Extensions
{
	public static class RedisExtension
	{
		public static RedisKey ToRedisKey(this string key)
		{
			return new RedisKey(key);
		}

		public static RedisValue ToRedisValue(this string value)
		{
			return new RedisValue(value);
		}

	}
}
