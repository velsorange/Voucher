using Domain.Model;
using Domain.Validator.Interface;

namespace Domain.Validator;

public class TimeRedeemValidator : ITimeRedeemValidator
{
    public ValidationResult IsValidRedeem(Voucher voucher)
    {
        if (voucher == null) throw new ArgumentNullException(nameof(voucher));

        if (!voucher.TimeValidity.HasValue)
        {
            throw new ArgumentNullException(nameof(voucher));
        }

        var now = DateTimeOffset.UtcNow;
        switch (voucher.TimeValidity.Value)
        {
            case TimeValidity.From:
                if (voucher.From!.Value > now)
                {
                    return ValidationResult.Error("Voucher is not valid yet.");
                }
                break;
            case TimeValidity.FromTo:
                if (voucher.From!.Value > now || voucher.To!.Value < now)
                {
                    return ValidationResult.Error("Voucher is not valid.");
                }
                break;
            case TimeValidity.To:
                if (voucher.To!.Value < now)
                {
                    return ValidationResult.Error("The voucher is no longer valid.");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return ValidationResult.Success();
    }
}