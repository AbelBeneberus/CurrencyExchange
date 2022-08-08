using CurrencyExchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyExchange.Infrastructure.Database.Configurations
{
	public class CurrencyExchangeConfiguration : IEntityTypeConfiguration<CurrencyExchangeRate>
	{
		public void Configure(EntityTypeBuilder<CurrencyExchangeRate> builder)
		{
			builder.HasKey(rate => rate.Id);

			builder.HasMany(rate => rate.Trades)
				.WithOne(transaction => transaction.UsedRate)
				.HasForeignKey(transaction => transaction.UsedRateId);

			builder.Property(rate => rate.Id)
				.ValueGeneratedOnAdd();

			builder.Property(rate => rate.From)
				.IsRequired();

			builder.Property(rate => rate.Date).
				IsRequired();

			builder.Property(rate => rate.TimeStamp)
				.IsRequired();

			builder.Property(rate => rate.Rate)
				.IsRequired()
				.HasPrecision(21,6);
			builder.Ignore(rate => rate.Events);
		}
	}
}
