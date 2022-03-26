using Domain.Model;
using Domain.Validator.Interface;

namespace Domain.Validator;

public class TypeRedeemValidatorFactory : ITypeRedeemValidatorFactory
{
    public ITypeRedeemValidator GetValidator(VoucherType voucherType)
    {
        return voucherType switch
        {
            VoucherType.Single => new SingleRedeemValidator(),
            VoucherType.Multiple => new MultipleRedeemValidator(),
            VoucherType.XTimes => new XTimesRedeemValidator(),
            _ => throw new ArgumentOutOfRangeException(nameof(voucherType), voucherType, null)
        };
    }
}