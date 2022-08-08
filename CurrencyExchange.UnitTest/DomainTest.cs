using CurrencyExchange.Domain.Exceptions;
using CurrencyExchange.Domain.Services;
using CurrencyExchange.UnitTest.Helper;
using FluentAssertions;

namespace CurrencyExchange.UnitTest
{
	public class DomainTest
	{
		[Fact]
		public void CreateNewCurrencyTradeTransaction_GivenValidCurrencyExchangeRate_ShouldValidTransaction()
		{
			#region Arrange

			decimal rate = 2;
			CurrencyExchangeRateBuilder rateBuilder = new CurrencyExchangeRateBuilder();
			var exchangeRate = rateBuilder
				.WithValidToDate(DateTime.Now.AddHours(2))
				.WithRate(2)
				.Build();

			ExchangeComputationService computationService = new ExchangeComputationService();
			decimal amountToBeConverted = 1000;
			Guid correlationId = Guid.NewGuid();
			string userId = "user_1";

			#endregion

			#region Act

			var transaction =
				computationService.ComputeConversion(exchangeRate, amountToBeConverted, correlationId, userId);

			#endregion

			#region Assert

			var expectedConvertedAmount = rate * amountToBeConverted;
			transaction.Should().NotBeNull();
			transaction.AmountToBeConverted.Should().Be(amountToBeConverted,
				$"because we are passing {amountToBeConverted} for computation");
			transaction.ConvertedAmount.Should()
				.Be(expectedConvertedAmount, $"we are using conversion rate amount = {rate}");

			#endregion
		}


		[Fact]
		public void CreateNewCurrencyTradeTransaction_GivenExpiredCurrencyExchangeRate_ShouldThrowException()
		{
			#region Arrange

			decimal rate = 2;
			CurrencyExchangeRateBuilder rateBuilder = new CurrencyExchangeRateBuilder();
			var exchangeRate = rateBuilder
				.WithValidToDate(DateTime.Now.AddHours(-2))
				.WithRate(rate)
				.Build();

			ExchangeComputationService computationService = new ExchangeComputationService();
			decimal amountToBeConverted = 1000;
			Guid correlationId = Guid.NewGuid();
			string userId = "user_1";

			#endregion

			#region Act with Assertion

			computationService
				.Invoking(cs => cs.ComputeConversion(exchangeRate, amountToBeConverted, correlationId, userId))
				.Should()
				.Throw<ExpiredExchangeRateException>()
				.WithMessage($"The Exchange rate used have already expired.");

			#endregion

		}
	}
}