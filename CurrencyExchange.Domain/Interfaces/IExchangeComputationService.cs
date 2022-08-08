using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Domain.Interfaces
{
	public interface IExchangeComputationService
	{
		CurrencyTradeTransaction ComputeConversion(CurrencyExchangeRate exchangeRate, decimal amount,
			Guid correlationId, string userId);
	}
}
