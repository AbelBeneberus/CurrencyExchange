namespace CurrencyExchange.Application
{
	/// <summary>
	/// Store the initial traded time of the user and the consecutive successful trade counts
	/// </summary>
	public struct RateLimiterCounter
	{
		public DateTime Timestamp { get; set; }

		public double Count { get; set; }
	}
}
