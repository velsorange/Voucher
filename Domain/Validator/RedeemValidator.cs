using Domain.Model;
using Domain.Validator.Interface;

namespace Domain.Validator;

public class RedeemValidator : IRedeemValidator
{
    private readonly ITypeRedeemValidatorFactory _factory;
    private readonly ITimeRedeemValidator _timeRedeemValidator;

    public RedeemValidator(ITypeRedeemValidatorFactory factory, ITimeRedeemValidator timeRedeemValidator)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _timeRedeemValidator = timeRedeemValidator ?? throw new ArgumentNullException(nameof(timeRedeemValidator));
    }

    public ValidationResult IsValidRedeem(Voucher voucher)
    {
        if (voucher == null) throw new ArgumentNullException(nameof(voucher));

        var typeValidator = _factory.GetValidator(voucher.Type);
        var result = typeValidator.IsValidRedeem(voucher);

        if (!voucher.TimeValidity.HasValue)
        {
            return result;
        }

        var timeResult = _timeRedeemValidator.IsValidRedeem(voucher);
        if (!timeResult.IsSuccess)
        {
            result.AddError(timeResult.ErrorMessage);
        }

        return result;
    }
}