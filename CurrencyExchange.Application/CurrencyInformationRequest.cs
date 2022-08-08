namespace CurrencyExchange.Application
{

	public class CurrencyInformationRequest
	{
		public string Base { get; set; } = string.Empty;
		public string Symbols { get; set; }	= String.Empty;
		public override string ToString()
		{
			return $"?{nameof(Base).ToLowerInvariant()}={Base}&{nameof(Symbols).ToLowerInvariant()}={Symbols}";
		}
	}
}
