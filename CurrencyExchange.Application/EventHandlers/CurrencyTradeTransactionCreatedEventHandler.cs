using CurrencyExchange.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyExchange.Application.EventHandlers;

public class CurrencyTradeTransactionCreatedEventHandler : INotificationHandler<CurrencyExchangeEventNotification<CurrencyTradeTransactionCreatedEvent>>
{
	private readonly ILogger _logger;
	public CurrencyTradeTransactionCreatedEventHandler(ILogger<CurrencyTradeTransactionCreatedEventHandler> logger)
	{
		_logger = logger;
	}
	public Task Handle(CurrencyExchangeEventNotification<CurrencyTradeTransactionCreatedEvent> notification, CancellationToken cancellationToken)
	{
		var @event = notification.Event;

		_logger.LogInformation("Currency Trade Event: {Event}", @event.GetType().Name);
		// store events here.

		return Task.CompletedTask;
	}
}