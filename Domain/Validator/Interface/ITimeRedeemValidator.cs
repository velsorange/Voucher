using Domain.Model;

namespace Domain.Validator.Interface;

public interface ITimeRedeemValidator
{
    ValidationResult IsValidRedeem(Voucher voucher);
}