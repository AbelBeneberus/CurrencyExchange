using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Domain.Events;
using CurrencyExchange.Domain.Interfaces;

namespace CurrencyExchange.Infrastructure.Services
{
	public class CurrencyExchangeRateRepository : ICurrencyExchangeRateRepository
	{
		private readonly ICurrencyExchangeDbContext _context;
		public CurrencyExchangeRateRepository(ICurrencyExchangeDbContext context)
		{
			_context = context;
		}
		public async Task<int> CreateCurrencyExchangeRateAsync(CurrencyExchangeRate exchangeRate, CancellationToken cancellationToken)
		{
			exchangeRate.Events.Add(new CurrencyExchangeRateCreatedEvent(exchangeRate));
			await _context.CurrencyExchangeRate.AddAsync(exchangeRate);
			return await _context.SaveChangesAsync(cancellationToken);
		}
	}
}
