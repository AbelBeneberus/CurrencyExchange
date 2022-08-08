using MediatR;

namespace CurrencyExchange.Application.Commands.TradeCurrencyExchangeCommand
{
	public class TradeCurrencyExchangeCommand : IRequest<string>
	{
		public Guid CorrelationId { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public decimal Amount { get; set; }
		public string UserId { get; set; }
	}
}