using CurrencyExchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyExchange.Infrastructure.Database.Configurations;

public class CurrencyTradeTransactionEntityConfiguration : IEntityTypeConfiguration<CurrencyTradeTransaction>
{
	public void Configure(EntityTypeBuilder<CurrencyTradeTransaction> builder)
	{
		builder.HasKey(transaction => transaction.Id);

		builder.Property(transaction => transaction.Id)
			.ValueGeneratedOnAdd();

		builder.HasOne(transaction => transaction.UsedRate)
			.WithMany(rate => rate.Trades)
			.HasForeignKey(transaction => transaction.UsedRateId);

		builder.Property(transaction => transaction.From)
			.IsRequired();

		builder.Property(transaction => transaction.To)
			.IsRequired();

		builder.Property(transaction => transaction.UserId)
			.IsRequired();

		builder.Property(transaction => transaction.AmountToBeConverted)
			.IsRequired()
			.HasPrecision(21, 6);

		builder.Property(transaction => transaction.ConvertedAmount)
			.IsRequired()
			.HasPrecision(21, 6);

		builder.Ignore(transaction => transaction.Events);

	}
}