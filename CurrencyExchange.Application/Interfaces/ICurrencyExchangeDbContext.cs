using CurrencyExchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Application.Interfaces
{
	public interface ICurrencyExchangeDbContext
	{
		DbSet<CurrencyTradeTransaction> CurrencyTradeTransaction { get; set; }
		DbSet<CurrencyExchangeRate> CurrencyExchangeRate { get; set; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
