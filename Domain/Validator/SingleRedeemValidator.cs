using Domain.Model;
using Domain.Validator.Interface;

namespace Domain.Validator;

public class SingleRedeemValidator : ITypeRedeemValidator
{
    public ValidationResult IsValidRedeem(Voucher voucher)
    {
        if (voucher == null) throw new ArgumentNullException(nameof(voucher));

        return voucher.Redemptions.Any()
            ? ValidationResult.Error("Voucher has already redeemed.")
            : ValidationResult.Success();
    }
}