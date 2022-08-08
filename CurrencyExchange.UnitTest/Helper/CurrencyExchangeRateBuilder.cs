using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.UnitTest.Helper
{
	public class CurrencyExchangeRateBuilder
	{
		private string _from = "USD";
		private string _to = "EUR";
		private double _timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		private DateTime _date = DateTime.Today;
		private decimal _rate = 0.5M;
		private DateTime _validFrom = DateTime.Now.AddMinutes(-30);
		private DateTime _validToDate = DateTime.Now.AddSeconds(-1);

		public CurrencyExchangeRateBuilder WithValidToDate(DateTime date)
		{
			_validToDate = date;
			return this;
		}

		public CurrencyExchangeRateBuilder WithRate(decimal rate)
		{
			_rate = rate;
			return this;
		}

		public CurrencyExchangeRateBuilder WithBase(string from)
		{
			_from = from;
			return this;
		}

		public CurrencyExchangeRateBuilder WithSource(string to)
		{
			_to = to;
			return this;
		}

		public CurrencyExchangeRate Build()
		{
			return new CurrencyExchangeRate()
			{
				ValidToDate = _validToDate,
				ValidFrom = _validFrom,
				Rate = _rate,
				CreatedDate = DateTime.Today,
				Date = _date,
				From = _from,
				To = _to,
				TimeStamp = _timeStamp
			};
		}
	}
}
