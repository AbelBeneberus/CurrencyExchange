using System.Net;
using CurrencyExchange.Application;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Domain.Interfaces;
using CurrencyExchange.Infrastructure.Contracts;
using CurrencyExchange.Infrastructure.Services;
using CurrencyExchange.UnitTest.Helper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace CurrencyExchange.UnitTest.Services
{
	public class CurrencyExchangeInfoServiceTest
	{
		private Mock<HttpMessageHandler> _responseMessageMock = new();
		private readonly Mock<ILogger<CurrencyExchangeInfoService>> _loggerMock = new();
		private readonly Mock<ICurrencyExchangeCacheProvider> _currencyExchangeCacheProviderMock = new();
		private readonly Mock<ICurrencyExchangeRateRepository> _currencyExchangeRateRepositoryMock = new();

		[Fact]
		public async Task GetCurrencyExchangeInfo_GivenCachedRate_ShouldReturnExchangeRateFromCache()
		{
			#region Arrange

			CurrencyInformationRequest request = new CurrencyInformationRequest();
			CancellationToken cancellationToken = CancellationToken.None;
			CurrencyExchangeRateBuilder rateBuilder = new CurrencyExchangeRateBuilder();
			CurrencyExchangeRate exchangeRate = rateBuilder.Build();

			_currencyExchangeCacheProviderMock
				.Setup(cacheProvider => cacheProvider.GetCachedCurrencyExchangeAsync(It.IsAny<string>()))
				.ReturnsAsync(exchangeRate);

			CurrencyExchangeInfoService service = new CurrencyExchangeInfoService(
				new HttpClient(_responseMessageMock.Object), _loggerMock.Object,
				_currencyExchangeCacheProviderMock.Object, _currencyExchangeRateRepositoryMock.Object, ApplicationConfigurationProvider.GetApplicationConfiguration());

			#endregion


			#region Act

			var exchangeInfo = await service.GetExchangeRatesAsync(request, cancellationToken);

			#endregion

			#region Assert

			exchangeInfo.Rate.Should().Be(0.5M, $"0.5M is a default value for {nameof(CurrencyExchangeRateBuilder)}");
			exchangeInfo.ValidToDate.Should().Be(exchangeRate.ValidToDate);

			_currencyExchangeCacheProviderMock.Verify(
				get => get.GetCachedCurrencyExchangeAsync(It.IsAny<string>()),
				Times.Once);

			_currencyExchangeRateRepositoryMock.Verify(
				get => get.CreateCurrencyExchangeRateAsync(It.IsAny<CurrencyExchangeRate>(),
					It.IsAny<CancellationToken>()), Times.Never);

			#endregion

		}

		[Fact]
		public async Task GetCurrencyExchangeInfo_GivenNonCachedRate_ShouldHaveToFetchRateFromRemoteApi()
		{
			#region Arrange

			CurrencyInformationRequest request = new CurrencyInformationRequest()
			{
				Base = "USD",
				Symbols = "EUR"
			};
			CancellationToken cancellationToken = CancellationToken.None;
			CurrencyExchangeRateBuilder rateBuilder = new CurrencyExchangeRateBuilder();
			CurrencyExchangeRate exchangeRate = rateBuilder.Build();

			var response = new ExchangeRateResponse(exchangeRate.From,
				exchangeRate.TimeStamp,
				exchangeRate.Date,
				new Dictionary<string, decimal>()
				{
					{
						exchangeRate.To, exchangeRate.Rate
					}
				});

			_responseMessageMock = HttpClientHelper.GetResults(response);
			HttpClient client = new HttpClient(_responseMessageMock.Object);
			client.BaseAddress = new Uri("https://localhost");

			_currencyExchangeRateRepositoryMock.Setup(repo =>
					repo.CreateCurrencyExchangeRateAsync(It.IsAny<CurrencyExchangeRate>(),
						It.IsAny<CancellationToken>()))
				.ReturnsAsync(0);

			CurrencyExchangeInfoService service = new CurrencyExchangeInfoService(
				client,
				_loggerMock.Object,
				_currencyExchangeCacheProviderMock.Object,
				_currencyExchangeRateRepositoryMock.Object,
				ApplicationConfigurationProvider.GetApplicationConfiguration());


			#endregion


			#region Act

			var exchangeInfo = await service.GetExchangeRatesAsync(request, cancellationToken);

			#endregion

			#region Assert

			exchangeInfo.Rate.Should().Be(0.5M, $"0.5M is a default value for {nameof(CurrencyExchangeRateBuilder)}");
			exchangeInfo.From.Should().Be("USD");
			exchangeInfo.To.Should().Be("EUR");

			_currencyExchangeCacheProviderMock.Verify(
				get => get.GetCachedCurrencyExchangeAsync(It.IsAny<string>()),
				Times.Once);
			_currencyExchangeCacheProviderMock.Verify(
				get => get.CacheCurrencyExchangeAsync(It.IsAny<string>(),
					It.IsAny<CurrencyExchangeRate>(),
					It.IsAny<TimeSpan>()),
				Times.Once);

			_currencyExchangeRateRepositoryMock.Verify(
				get => get.CreateCurrencyExchangeRateAsync(It.IsAny<CurrencyExchangeRate>(),
					It.IsAny<CancellationToken>()), Times.Once);

			#endregion

		}
	}
}
