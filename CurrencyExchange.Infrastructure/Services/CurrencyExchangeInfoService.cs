using CurrencyExchange.Application;
using CurrencyExchange.Application.Configurations;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Interfaces;
using CurrencyExchange.Infrastructure.Extensions;
using CurrencyExchange.Infrastructure.Helper;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using System.Net.Http.Json;
using CurrencyExchange.Infrastructure.Contracts;

namespace CurrencyExchange.Infrastructure.Services;

public class CurrencyExchangeInfoService : ICurrencyExchangeInfoService
{
	private readonly HttpClient _httpClient;
	private readonly ILogger _logger;
	private readonly ICurrencyExchangeCacheProvider _currencyExchangeCacheProvider;
	private readonly ICurrencyExchangeRateRepository _currencyExchangeRateRepository;
	private readonly ApplicationConfiguration _applicationConfiguration;
	public CurrencyExchangeInfoService(HttpClient httpClient,
		ILogger<CurrencyExchangeInfoService> logger,
		ICurrencyExchangeCacheProvider currencyExchangeCacheProvider,
		ICurrencyExchangeRateRepository currencyExchangeRateRepository,
		ApplicationConfiguration applicationConfiguration)
	{
		_httpClient = httpClient;
		_logger = logger;
		_currencyExchangeCacheProvider = currencyExchangeCacheProvider;
		_currencyExchangeRateRepository = currencyExchangeRateRepository;
		_applicationConfiguration = applicationConfiguration;
	}

	public async Task<Domain.Entities.CurrencyExchangeRate> GetExchangeRatesAsync(CurrencyInformationRequest currencyInformationRequest, CancellationToken cancellationToken)
	{
		try
		{
			var key = RedisKeyProvider.CurrencyExchangeRateKey(currencyInformationRequest.Base,
				currencyInformationRequest.Symbols);
			var cachedValue = await _currencyExchangeCacheProvider.GetCachedCurrencyExchangeAsync(key);

			if (cachedValue != null) return cachedValue;

			var response =
				await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(currencyInformationRequest.ToString(),
					cancellationToken: cancellationToken);

			var domainExchangeInfo = response?.ToDomainCurrencyExchangeRate();

			if (domainExchangeInfo == null)
			{
				return new Domain.Entities.CurrencyExchangeRate();
			}

			var result =
				await _currencyExchangeRateRepository.CreateCurrencyExchangeRateAsync(domainExchangeInfo,
					cancellationToken);
			domainExchangeInfo.Id = result;

			await _currencyExchangeCacheProvider.CacheCurrencyExchangeAsync(key,
				domainExchangeInfo, TimeSpan.FromMinutes(30));

			return domainExchangeInfo;
		}
		catch (BrokenCircuitException exception)
		{
			_logger.LogError(exception,
				"Remote Exchange Provider Service is inoperative");
			throw;
		}

	}

}