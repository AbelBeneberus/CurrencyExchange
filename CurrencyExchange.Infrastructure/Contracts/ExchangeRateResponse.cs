using System.Text.Json.Serialization;

namespace CurrencyExchange.Infrastructure.Contracts
{
	public class ExchangeRateResponse
	{
		public string Base { get; }
		public double TimeStamp { get; }
		public DateTime Date { get; }
		public Dictionary<string, decimal> Rates { get; }

		[JsonConstructor]
		public ExchangeRateResponse(string @base, double timestamp, DateTime date, Dictionary<string, decimal> rates)
		{
			Base = @base;
			TimeStamp = timestamp;
			Date = date;
			Rates = rates;
		}
	}
}
