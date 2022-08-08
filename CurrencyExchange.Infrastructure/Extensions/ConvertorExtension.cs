using CurrencyExchange.Application;
using CurrencyExchange.Infrastructure.Contracts;

namespace CurrencyExchange.Infrastructure.Extensions
{
	public static class ConvertorExtension
	{
		public static Domain.Entities.CurrencyExchangeRate ToDomainCurrencyExchangeRate(this ExchangeRateResponse exchangeRate)
		{
			return new Domain.Entities.CurrencyExchangeRate()
			{
				TimeStamp = exchangeRate.TimeStamp,
				From = exchangeRate.Base,
				To = exchangeRate.Rates.First().Key,
				Date = exchangeRate.Date,
				CreatedDate = exchangeRate.Date,
				Rate = exchangeRate.Rates.First().Value,
				ValidFrom = exchangeRate.TimeStamp
					.UnixTimeStampToDateTime(),
				ValidToDate = exchangeRate.TimeStamp
					.UnixTimeStampToDateTime()
					.AddMinutes(30)
			};
		}
	}
}
