namespace CurrencyExchange.Domain.Exceptions
{
	public class ExpiredExchangeRateException : Exception
	{
		public ExpiredExchangeRateException() : base($"The Exchange rate used have already expired.")
		{

		}
	}
}
