using System.Reflection;
using CurrencyExchange.Domain.Interfaces;
using CurrencyExchange.Domain.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchange.Application
{
	public static class DependencyModule
	{
		public static void AddApplicationModule(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IExchangeComputationService, ExchangeComputationService>();
			serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
			serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

		}
	}
}
