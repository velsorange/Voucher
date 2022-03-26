using Domain.Model;

namespace Domain.Validator.Interface;

public interface ITypeRedeemValidatorFactory
{
    ITypeRedeemValidator GetValidator(VoucherType voucherType);
}