using Domain.Model.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Model;

public class Redemption : IAuditEntity
{
    public Guid Id { get; set; }
    public Guid VoucherId { get; set; }
    public Voucher Voucher { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class RedemptionConfiguration : IEntityTypeConfiguration<Redemption>
{
    public void Configure(EntityTypeBuilder<Redemption> builder)
    {
        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Id).IsRequired();

        builder.HasOne(x => x.Voucher)
            .WithMany(x => x.Redemptions)
            .HasForeignKey(x => x.VoucherId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}