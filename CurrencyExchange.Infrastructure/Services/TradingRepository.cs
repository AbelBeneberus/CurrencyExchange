using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Domain.Events;
using CurrencyExchange.Domain.Interfaces;

namespace CurrencyExchange.Infrastructure.Services
{
	public class TradingRepository : ITradingRepository
	{
		private readonly ICurrencyExchangeDbContext _currencyExchangeDbContext;
		public TradingRepository(ICurrencyExchangeDbContext currencyExchangeDbContext)
		{
			_currencyExchangeDbContext = currencyExchangeDbContext;
		}
		public Task CreateCurrencyExchangeTransaction(CurrencyTradeTransaction transaction,
			CancellationToken cancellationToken)
		{
			transaction.Events.Add(new CurrencyTradeTransactionCreatedEvent(transaction));
			_currencyExchangeDbContext.CurrencyTradeTransaction.Add(transaction);
			return _currencyExchangeDbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
