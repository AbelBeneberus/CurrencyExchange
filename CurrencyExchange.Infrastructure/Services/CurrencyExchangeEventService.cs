using CurrencyExchange.Application;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyExchange.Infrastructure.Services
{
	public class CurrencyExchangeEventService : ICurrencyExchangeEventService
	{
		private readonly ILogger _logger;
		private readonly IPublisher _publisher;
		public CurrencyExchangeEventService(ILogger<CurrencyExchangeEventService> logger, IPublisher publisher)
		{
			_logger = logger;
			_publisher = publisher;
		}
		public async Task PublishAsync(CurrencyExchangeRateBaseEvent @event)
		{
			_logger.LogInformation("Publishing domain event. Event - {event}", @event.GetType().Name);
			await _publisher.Publish(GetNotificationCorrespondingToDomainEvent(@event));
		}

		private INotification GetNotificationCorrespondingToDomainEvent(CurrencyExchangeRateBaseEvent @event)
		{
			return ((INotification)Activator.CreateInstance(
				typeof(CurrencyExchangeEventNotification<>).MakeGenericType(@event.GetType()), @event)!)!;
		}
	}
}
