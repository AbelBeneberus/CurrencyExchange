using System.Text.Json.Serialization;
using CurrencyExchange.Domain.Events;
using CurrencyExchange.Domain.Interfaces;

namespace CurrencyExchange.Domain.Entities
{

	public abstract class BaseEntityWithDomainEvent : IHasCurrencyExchangeDomainEvent
	{
		public int Id { get; set; }
		public DateTime? CreatedDate { get; set; }

		[JsonIgnore]
		public virtual List<CurrencyExchangeRateBaseEvent> Events { get; set; } = new();
	}
}
