using CurrencyExchange.Domain.Events;
using MediatR;

namespace CurrencyExchange.Application
{
	public class CurrencyExchangeEventNotification<TEvent>: INotification where TEvent :CurrencyExchangeRateBaseEvent
	{
		public CurrencyExchangeEventNotification(TEvent @event)
		{
			Event = @event; 
		}
		public TEvent Event { get; set; }
	}
}
