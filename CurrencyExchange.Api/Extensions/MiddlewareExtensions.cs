using CurrencyExchange.Api.Middleware;

namespace CurrencyExchange.Api.Extensions
{
	public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseExchangeTransactionLimiter(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExchangeTransactionRateLimiter>();
		}
	}
}
