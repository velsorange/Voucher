using Domain.Model.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Model;

public class Voucher : IAuditEntity
{
    public Guid Id { get; set; }
    public VoucherType Type { get; set; }
    public int? MaxRedemptionCount { get; set; }
    public TimeValidity? TimeValidity { get; set; }
    public DateTimeOffset? From { get; set; }
    public DateTimeOffset? To { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public virtual List<Redemption> Redemptions { get; set; } = new List<Redemption>();
}

public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> builder)
    {
        builder.HasKey(x => new {x.Id});
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Type).HasConversion<string>();
        builder.Property(x => x.TimeValidity).HasConversion<string>();
        
        builder.HasMany(x => x.Redemptions)
            .WithOne(x => x.Voucher)
            .HasForeignKey(x => x.VoucherId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}