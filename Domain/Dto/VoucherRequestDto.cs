using Domain.Model;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dto;

public class VoucherRequestDto : IValidatableObject
{
    public VoucherType Type { get; set; }
    public int? MaxRedemptionCount { get; set; }
    public TimeValidity? TimeValidity { get; set; }
    public DateTimeOffset? From { get; set; }
    public DateTimeOffset? To { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Type == VoucherType.XTimes && MaxRedemptionCount == null)
        {
            yield return new ValidationResult("XTimes requires MaxRedemptionCount.");
        }

        if (!TimeValidity.HasValue) yield break;

        switch (TimeValidity.Value)
        {
            case Model.TimeValidity.From:
                if (From == null)
                {
                    yield return new ValidationResult($"TimeValidity.{nameof(Model.TimeValidity.From)} requires {nameof(From)}.");
                }
                break;
            case Model.TimeValidity.FromTo:
                if (From == null)
                {
                    yield return new ValidationResult($"TimeValidity.{nameof(Model.TimeValidity.FromTo)} requires {nameof(From)}.");
                }
                if (To == null)
                {
                    yield return new ValidationResult($"TimeValidity.{nameof(Model.TimeValidity.FromTo)} requires {nameof(To)}.");
                }
                if (To <= From)
                {
                    yield return new ValidationResult($"TimeValidity.{nameof(Model.TimeValidity.FromTo)} requires earlier {nameof(From)} than {nameof(To)}.");
                }
                break;
            case Model.TimeValidity.To:
                if (To == null)
                {
                    yield return new ValidationResult($"TimeValidity.{nameof(Model.TimeValidity.To)} requires {nameof(To)}.");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}