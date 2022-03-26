using Domain.Model;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public class VoucherRepository : IVoucherRepository
{
    private readonly VoucherContext _context;

    public VoucherRepository(VoucherContext context)
    {
        _context = context;
    }

    public async Task<Voucher> Create(Voucher voucher, CancellationToken ct)
    {
        await _context.AddAsync(voucher, ct);
        return voucher;
    }

    public async Task Redeem(Redemption redemption, CancellationToken ct)
    {
        await _context.AddAsync(redemption, ct);
    }

    public async Task Save(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }

    public Voucher GetVoucher(Guid voucherId)
    {
        return _context.Vouchers
            .Include(x => x.Redemptions).FirstOrDefault(x => x.Id == voucherId);
    }
}