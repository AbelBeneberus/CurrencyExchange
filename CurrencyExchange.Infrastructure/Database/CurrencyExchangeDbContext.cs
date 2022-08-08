using System.Reflection;
using CurrencyExchange.Application.Configurations;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyExchange.Infrastructure.Database
{
	public class CurrencyExchangeDbContext : DbContext, ICurrencyExchangeDbContext
	{
		private readonly ApplicationConfiguration _applicationConfiguration;
		private readonly ILogger _logger;
		private readonly ICurrencyExchangeEventService _currencyExchangeEventService;
		public DbSet<CurrencyTradeTransaction> CurrencyTradeTransaction { get; set; }
		public DbSet<CurrencyExchangeRate> CurrencyExchangeRate { get; set; }


		public CurrencyExchangeDbContext(ApplicationConfiguration applicationConfiguration, ILogger<CurrencyExchangeDbContext> logger, ICurrencyExchangeEventService currencyExchangeEventService)
		{
			_applicationConfiguration = applicationConfiguration;
			_logger = logger;
			_currencyExchangeEventService = currencyExchangeEventService;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_applicationConfiguration.ConnectionStrings.CurrencyExchangeDb);
		}
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntityWithDomainEvent> entry in ChangeTracker.Entries<BaseEntityWithDomainEvent>())
			{
				if (entry.State == EntityState.Added)
				{
					entry.Entity.CreatedDate = DateTime.UtcNow;
				}
			}


			var result = await base.SaveChangesAsync(cancellationToken);

			await DispatchEvents();
			return result;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
		private async Task DispatchEvents()
		{
			while (true)
			{
				var currencyExchangeRateBaseEvent = ChangeTracker.Entries<IHasCurrencyExchangeDomainEvent>()
					.Select(x => x.Entity.Events)
					.SelectMany(x => x)
					.Where(@event => !@event.IsPublished)
					.FirstOrDefault();
				if (currencyExchangeRateBaseEvent == null) break;

				currencyExchangeRateBaseEvent.IsPublished = true;
				await _currencyExchangeEventService.PublishAsync(currencyExchangeRateBaseEvent);
			}
		}
	}
}
