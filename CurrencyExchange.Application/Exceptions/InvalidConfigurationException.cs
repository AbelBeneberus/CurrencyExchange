namespace CurrencyExchange.Application.Exceptions
{
	public class InvalidConfigurationException : Exception
	{
		public InvalidConfigurationException(string message, Exception exception) : base(message, exception)
		{

		}
	}
}
