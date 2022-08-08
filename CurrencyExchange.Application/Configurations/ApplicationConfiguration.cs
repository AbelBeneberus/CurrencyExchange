namespace CurrencyExchange.Application.Configurations
{
	public class ApplicationConfiguration
	{
		public ConnectionString ConnectionStrings { get; set; } = new ConnectionString();
		public ExchangeProvidersConfiguration ExchangeProviders { get; set; } = new ExchangeProvidersConfiguration();
		public PolicyOptions PolicyOptions { get; set; } = new PolicyOptions();
		public RedisConfiguration Redis { get; set; } = new RedisConfiguration();
	}
}
