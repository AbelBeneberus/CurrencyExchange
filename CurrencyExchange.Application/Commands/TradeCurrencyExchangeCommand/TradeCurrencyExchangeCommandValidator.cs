using FluentValidation;

namespace CurrencyExchange.Application.Commands.TradeCurrencyExchangeCommand
{
	public class TradeCurrencyExchangeCommandValidator : AbstractValidator<TradeCurrencyExchangeCommand>
	{
		public TradeCurrencyExchangeCommandValidator()
		{
			RuleFor(tradeCommand => tradeCommand.From)
				.NotNull()
				.NotEmpty();
			RuleFor(tradeCommand => tradeCommand.To)
				.NotNull()
				.NotEmpty();
			RuleFor(tradeCommand => tradeCommand.Amount)
				.NotEqual(0);
			RuleFor(tradeCommand => tradeCommand.UserId)
				.NotNull()
				.NotEmpty();
			RuleFor(tradeCommand => tradeCommand.CorrelationId)
				.NotNull()
				.NotEmpty();

		}
	}
}
