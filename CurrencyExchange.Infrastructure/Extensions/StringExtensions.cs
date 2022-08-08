using System.Net;
using CurrencyExchange.Application.Configurations;
using CurrencyExchange.Application.Exceptions;

namespace CurrencyExchange.Infrastructure.Extensions
{
	public static class StringExtensions
	{
		public static DnsEndPoint ToDnsEndpoint(this string host)
		{
			try
			{
				var endpointInformation = host.Split(':');

				var address = endpointInformation[0];
				var isValidPort = int.TryParse(endpointInformation[1], out int port);
				return new DnsEndPoint(endpointInformation.First(), port);
			}
			catch (Exception e)
			{
				throw new InvalidConfigurationException(
					$"Unable to resolve host address {host}, please check configuration of {nameof(RedisConfiguration)}", e);
			}

		}
	}
}
