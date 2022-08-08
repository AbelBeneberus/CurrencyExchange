using CurrencyExchange.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyExchange.Application.EventHandlers
{
	public class CurrencyExchangeRateCreatedEventHandler	: INotificationHandler<CurrencyExchangeEventNotification<CurrencyExchangeRateCreatedEvent>>
	{
		private readonly ILogger _logger;

		public CurrencyExchangeRateCreatedEventHandler(ILogger<CurrencyExchangeRateCreatedEventHandler> logger)
		{
			_logger = logger;
		}

		public Task Handle(CurrencyExchangeEventNotification<CurrencyExchangeRateCreatedEvent> notification, CancellationToken cancellationToken)
		{
			var @event = notification.Event;

			_logger.LogInformation("Currency Exchange Rate Event: {Event}", @event.GetType().Name);

			return Task.CompletedTask;
		}
	}
}
