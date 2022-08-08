using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Application.Interfaces
{
	public interface ICurrencyExchangeInfoService
	{
		Task<CurrencyExchangeRate> GetExchangeRatesAsync(CurrencyInformationRequest currencyInformationRequest,
			CancellationToken cancellationToken);
	}
}
