using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyExchange.Application.Configurations;

namespace CurrencyExchange.UnitTest.Helper
{
	public static class ApplicationConfigurationProvider
	{
		public static ApplicationConfiguration GetApplicationConfiguration()
		{
			return new ApplicationConfiguration()
			{
				ConnectionStrings = new ConnectionString()
				{
					CurrencyExchangeDb = "sql"
				},
				PolicyOptions = new PolicyOptions()
				{
					RetryCount = 1,
					ExceptionsAllowedBeforeBreaking = 1,
					DurationOfBreak = 1,
					MaximumDelay = 1,
					MedianFirstRetryDelay = 1
				},
				ExchangeProviders = new ExchangeProvidersConfiguration()
				{
					ApiKey = "key1",
					ExchangeProviderEndpoint = "exchange.com"
				},
				Redis = new RedisConfiguration()
				{
					Hosts = new[] { "localhost:1123" }
				}
			};
		}
	}
}
