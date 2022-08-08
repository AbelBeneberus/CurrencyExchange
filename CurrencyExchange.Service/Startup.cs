
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchange.Service
{
	public class Startup
	{
		private const string API_KEY = "wct0bB9UbQzttDQATPwb6QV2nWUQeeJu"; 
		static void Main(string[] args)
		{
			//ConfigureServices();
		}

		static void ConfigureServices(IServiceCollection serviceCollections)
		{
			serviceCollections.AddHttpClient(); 
		}
	}
}