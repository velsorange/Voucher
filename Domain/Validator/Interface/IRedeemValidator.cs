using Domain.Model;

namespace Domain.Validator.Interface;

public interface IRedeemValidator
{
    ValidationResult IsValidRedeem(Voucher voucher);
}