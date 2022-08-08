using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Interfaces;
using MediatR;

namespace CurrencyExchange.Application.Commands.TradeCurrencyExchangeCommand;

public class TradeCurrencyExchangeCommandHandler : IRequestHandler<TradeCurrencyExchangeCommand, string>
{
	private readonly ICurrencyExchangeInfoService _currencyExchangeInfoService;
	private readonly ITradingRepository _tradingRepository;
	private readonly IExchangeComputationService _exchangeComputationService;
	public TradeCurrencyExchangeCommandHandler(ICurrencyExchangeInfoService currencyExchangeInfoService, ITradingRepository tradingRepository, IExchangeComputationService exchangeComputationService)
	{
		_currencyExchangeInfoService = currencyExchangeInfoService;
		_tradingRepository = tradingRepository;
		_exchangeComputationService = exchangeComputationService;
	}
	public async Task<string> Handle(TradeCurrencyExchangeCommand request, CancellationToken cancellationToken)
	{
		var currencyInfo = await _currencyExchangeInfoService
			.GetExchangeRatesAsync(
				new CurrencyInformationRequest()
				{
					Base = request.From,
					Symbols = request.To
				},
				cancellationToken);

		var transaction =
			_exchangeComputationService.ComputeConversion(currencyInfo,
				request.Amount,
				request.CorrelationId,
				request.UserId);


		await _tradingRepository.CreateCurrencyExchangeTransaction(transaction, cancellationToken);

		return
			$"{transaction.AmountToBeConverted} {request.From} have been converted to {transaction.ConvertedAmount} {request.To} with Rate of {currencyInfo.Rate}";
	}
}