namespace CurrencyExchange.Application.Configurations
{
	public class PolicyOptions
	{
		public int DurationOfBreak { get; set; }

		public int ExceptionsAllowedBeforeBreaking { get; set; }

		public int RetryCount { get; set; }
		public int MaximumDelay { get; set; }
		public int MedianFirstRetryDelay { get; set; }
	}

}
