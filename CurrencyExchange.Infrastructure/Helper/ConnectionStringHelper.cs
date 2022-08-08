using CurrencyExchange.Application.Configurations;

namespace CurrencyExchange.Infrastructure.Helper
{
	public class ConnectionStringHelper
	{
		private static string? _sqlConnectionString;

		public ConnectionStringHelper(ApplicationConfiguration applicationConfiguration)
		{
			_sqlConnectionString = applicationConfiguration.ConnectionStrings.CurrencyExchangeDb ?? null;
		}

		public static string? GetSqlConnectionString()
		{
			return _sqlConnectionString;
		}
	}
}
