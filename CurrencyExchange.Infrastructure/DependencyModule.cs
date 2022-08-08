using System.Net;
using CurrencyExchange.Application.Configurations;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Interfaces;
using CurrencyExchange.Infrastructure.Database;
using CurrencyExchange.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using StackExchange.Redis;

namespace CurrencyExchange.Infrastructure
{
	public static class DependencyModule
	{
		public static void AddInfrastructureModule(this IServiceCollection serviceCollection, ApplicationConfiguration applicationConfiguration)
		{
			serviceCollection.AddHttpClient<ICurrencyExchangeInfoService, CurrencyExchangeInfoService>(
					client =>
					{
						client.BaseAddress = new Uri(applicationConfiguration.ExchangeProviders.ExchangeProviderEndpoint);
						client.DefaultRequestHeaders.Add("Accept", "application/json");
						client.DefaultRequestHeaders.Add("apikey", applicationConfiguration.ExchangeProviders.ApiKey);
					})
				.AddPolicyHandler(GetRetryPolicy(applicationConfiguration))
				.AddPolicyHandler(GetCircuitBreakerPolicy(applicationConfiguration));
		  
			serviceCollection.AddDbContext<CurrencyExchangeDbContext>();
		   
			serviceCollection.AddSingleton(sp => ConnectionMultiplexer.Connect(
				new ConfigurationOptions
				{
					EndPoints = { string.Join(',', applicationConfiguration.Redis.Hosts) },
					Ssl = false,
					AbortOnConnectFail = false,
					AllowAdmin = true,
					ReconnectRetryPolicy = new LinearRetry(10)
				}));

			serviceCollection.AddScoped<ICurrencyExchangeDbContext>(provider =>
				provider.GetService<CurrencyExchangeDbContext>() ?? throw new InvalidOperationException());
			serviceCollection.AddSingleton<ICurrencyExchangeCacheProvider, CurrencyExchangeCacheProvider>();
			serviceCollection.AddScoped<ITradingRepository, TradingRepository>();
			serviceCollection.AddScoped<ICurrencyExchangeRateRepository, CurrencyExchangeRateRepository>();
			serviceCollection.AddSingleton<IUserActivityTransactionProvider, UserTransactionRateLimiter>();
			serviceCollection.AddScoped<ICurrencyExchangeEventService, CurrencyExchangeEventService>();
		}

		private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ApplicationConfiguration applicationConfiguration)
		{
			// Jitter strategy for overcoming peaks of similar retries from many clients in partial outage.
			var maxDelay = TimeSpan
				.FromSeconds(applicationConfiguration.PolicyOptions.MaximumDelay);
			var delay = Backoff
				.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan
						.FromSeconds(applicationConfiguration.PolicyOptions.MedianFirstRetryDelay),
					retryCount: applicationConfiguration.PolicyOptions.RetryCount)
				.Select(s => TimeSpan
					.FromTicks(Math.Min(s.Ticks,
						maxDelay.Ticks)));

			return HttpPolicyExtensions
				.HandleTransientHttpError()
				.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
				.WaitAndRetryAsync(delay);
		}

		private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ApplicationConfiguration applicationConfiguration)
		{
			return HttpPolicyExtensions
				.HandleTransientHttpError()
				.CircuitBreakerAsync(applicationConfiguration.PolicyOptions.ExceptionsAllowedBeforeBreaking,
					TimeSpan.FromSeconds(applicationConfiguration.PolicyOptions.DurationOfBreak));
		}

	}
}
