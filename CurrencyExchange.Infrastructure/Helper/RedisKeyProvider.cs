namespace CurrencyExchange.Infrastructure.Helper
{
	public static class RedisKeyProvider
	{
		public static string CurrencyExchangeRateKey(string from, string to) => $"exchange:{from}:{to}";
		public static string ClientRateLimiterKey(string? clientId) => $"rateLimiter_{clientId}";
	}
}
