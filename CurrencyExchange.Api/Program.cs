using CurrencyExchange.Api.Extensions;
using CurrencyExchange.Api.Filters;
using CurrencyExchange.Application;
using CurrencyExchange.Application.Configurations;
using CurrencyExchange.Infrastructure;
using CurrencyExchange.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Api
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var applicationConfiguration = builder.Configuration.GetSection("application").Get<ApplicationConfiguration>();
			RegisterServices(builder.Services, applicationConfiguration);

			var app = builder.Build();

			ConfigureWebApplication(app);

			using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
			{
				if (serviceScope != null)

				{
					var context = serviceScope.ServiceProvider.GetRequiredService<CurrencyExchangeDbContext>();

					var isMigrationNeeded = (await context.Database.GetPendingMigrationsAsync()).Any();

					if (isMigrationNeeded)
					{
						context.Database.Migrate();
					}																							   
				}
			}

			await app.RunAsync();
		}

		private static void RegisterServices(IServiceCollection serviceCollection,
			ApplicationConfiguration applicationConfiguration)
		{
			serviceCollection.AddControllers();
			serviceCollection.AddEndpointsApiExplorer();
			serviceCollection.AddSwaggerGen(c => c.OperationFilter<SwaggerHeader>());
			serviceCollection.AddLogging(logging => logging.AddConsole());
			serviceCollection.AddApplicationModule();
			serviceCollection.AddInfrastructureModule(applicationConfiguration);
			serviceCollection.AddSingleton<ApplicationConfiguration>(applicationConfiguration);


		}

		private static void ConfigureWebApplication(WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{

				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseHttpLogging();

			app.UseAuthorization();

			app.MapControllers();
			app.UseExchangeTransactionLimiter();
		}
	}


}