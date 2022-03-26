using Domain.Model;

namespace Domain.Repository;

public interface IVoucherRepository
{
    Task<Voucher> Create(Voucher voucher, CancellationToken ct);
    Task Redeem(Redemption redemption, CancellationToken ct);
    Task Save(CancellationToken ct);
    Voucher GetVoucher(Guid voucherId);
}