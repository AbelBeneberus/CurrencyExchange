using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Domain.Exceptions;
using CurrencyExchange.Domain.Interfaces;

namespace CurrencyExchange.Domain.Services
{
	public class ExchangeComputationService : IExchangeComputationService
	{
		public CurrencyTradeTransaction ComputeConversion(CurrencyExchangeRate exchangeRate,
			decimal amount, Guid correlationId, string userId)
		{
			if (exchangeRate.ValidToDate < DateTime.Now)
			{
				throw new ExpiredExchangeRateException();
			}
			return new CurrencyTradeTransaction()
			{
				From = exchangeRate.From,
				To = exchangeRate.To,
				AmountToBeConverted = amount,
				ConvertedAmount = ComputeExchange(exchangeRate.Rate, amount),
				TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
				CorrelationId = correlationId,
				UserId = userId,
				TransactionDate = DateTime.Today,
				UsedRateId = exchangeRate.Id
			};
		}

		private decimal ComputeExchange(decimal rate, decimal amount)
		{
			return rate * amount;
		}
	}
}
