using System.Security.Cryptography;
using System.Text;
using CurrencyExchange.Application;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Infrastructure.Helper;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CurrencyExchange.Infrastructure.Services
{
	public class UserTransactionRateLimiter : IUserActivityTransactionProvider
	{
		private readonly ILogger _logger;
		private readonly IDatabaseAsync _redisDbAsync;

		private static readonly LuaScript _atomicIncrement = LuaScript.Prepare(
			"local count = redis.call(\"INCRBYFLOAT\", @key, tonumber(@delta)) local ttl = redis.call(\"TTL\", @key) if ttl == -1 then redis.call(\"EXPIRE\", @key, @timeout) end return count");

		public UserTransactionRateLimiter(ConnectionMultiplexer connectionMultiplexer, ILogger<UserTransactionRateLimiter> logger)
		{
			_redisDbAsync = connectionMultiplexer.GetDatabase();
			_logger = logger;
		}

		public async Task<RateLimiterCounter> ProcessesRequest(string? userId)
		{
			var interval = TimeSpan.FromHours(1);
			var now = DateTime.UtcNow;
			var numberOfIntervals = now.Ticks / interval.Ticks;
			var intervalStart = new DateTime(numberOfIntervals * interval.Ticks, DateTimeKind.Utc);

			_logger.LogDebug("Calling Lua script. {counterId}, {timeout}, {IncrementBy}", BuildCounterId(userId), interval.TotalSeconds,
				1D);

			var count = await _redisDbAsync.ScriptEvaluateAsync(_atomicIncrement,
				new
				{
					key = new RedisKey(BuildCounterId(userId)),
					timeout = interval.TotalSeconds,
					delta = 1
				});

			return new RateLimiterCounter
			{
				Count = (double)count,
				Timestamp = intervalStart
			};
		}

		private string BuildCounterId(string? userId)
		{
			var key = RedisKeyProvider.ClientRateLimiterKey(userId);
			var bytes = Encoding.UTF8.GetBytes(key);
			using var algorithm = SHA1.Create();
			var hash = algorithm.ComputeHash(bytes);
			var counterId = Convert.ToBase64String(hash);
			return counterId;
		}
	}
}
