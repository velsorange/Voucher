using Domain.Model;

namespace Domain.Validator.Interface;

public interface ITypeRedeemValidator
{
    ValidationResult IsValidRedeem(Voucher voucher);
}