using System.Net;
using CurrencyExchange.Application.Interfaces;

namespace CurrencyExchange.Api.Middleware
{
	public class ExchangeTransactionRateLimiter
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;
		private readonly IUserActivityTransactionProvider _activityTransactionProvider;
		private const string HeaderName = "M-direct-client";
		public ExchangeTransactionRateLimiter(RequestDelegate next,
			ILogger<ExchangeTransactionRateLimiter> logger,
			IUserActivityTransactionProvider activityTransactionProvider)
		{
			_next = next;
			_logger = logger;
			_activityTransactionProvider = activityTransactionProvider;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			string? clientId = ResolveClientId(httpContext);
			if (string.IsNullOrEmpty(clientId))
			{
				httpContext.Response.Clear();
				httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				await httpContext.Response.WriteAsync("Unauthorized");
			}

			var rateLimitCount = await _activityTransactionProvider.ProcessesRequest(clientId);
			if (rateLimitCount.Count > 10 && rateLimitCount.Timestamp < DateTime.Now)
			{
				await TransactionExceededResponse(httpContext,
					(DateTime.Now - rateLimitCount.Timestamp).Minutes.ToString());
			}
			await _next(httpContext);
		}

		private string? ResolveClientId(HttpContext httpContext)
		{
			string? clientId = null;
			if (httpContext.Request.Headers.TryGetValue(HeaderName, out var values))
			{
				clientId = values.First();
			}
			return clientId;
		}

		public Task TransactionExceededResponse(HttpContext httpContext, string retryAfter)
		{

			var message = $"Currency exchange trade transaction exceeded! maximum admitted 10 per hour. please retry after {retryAfter}";

			httpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests; ;
			httpContext.Response.ContentType = "text/plain";
			httpContext.Response.Headers.RetryAfter = retryAfter;

			return httpContext.Response.WriteAsync(message);
		}
	}
}
