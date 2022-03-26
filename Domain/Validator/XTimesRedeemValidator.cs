using Domain.Model;
using Domain.Validator.Interface;

namespace Domain.Validator;

public class XTimesRedeemValidator : ITypeRedeemValidator
{
    public ValidationResult IsValidRedeem(Voucher voucher)
    {
        if (voucher == null) throw new ArgumentNullException(nameof(voucher));

        return voucher.Redemptions.Count >= voucher.MaxRedemptionCount
            ? ValidationResult.Error("The voucher is redeemed in the authorized quantity.")
            : ValidationResult.Success();
    }
}