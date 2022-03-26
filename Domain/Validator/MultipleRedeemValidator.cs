using Domain.Model;
using Domain.Validator.Interface;

namespace Domain.Validator;

public class MultipleRedeemValidator : ITypeRedeemValidator
{
    public ValidationResult IsValidRedeem(Voucher voucher)
    {
        if (voucher == null) throw new ArgumentNullException(nameof(voucher));

        return ValidationResult.Success();
    }
}