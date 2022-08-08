namespace CurrencyExchange.Application.Interfaces
{
	public interface IUserActivityTransactionProvider
	{
		Task<RateLimiterCounter> ProcessesRequest(string? clientId);
	}
}
