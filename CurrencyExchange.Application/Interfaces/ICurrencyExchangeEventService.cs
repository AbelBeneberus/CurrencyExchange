using CurrencyExchange.Domain.Events;

namespace CurrencyExchange.Application.Interfaces
{
	public interface ICurrencyExchangeEventService
	{
		Task PublishAsync(CurrencyExchangeRateBaseEvent @event);
	}
}
