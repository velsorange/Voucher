using Domain.Model;

namespace Domain.Dto;

public class VoucherDto
{
    public Guid Id { get; set; }
    public VoucherType Type { get; set; }
    public int? MaxRedemptionCount { get; set; }
    public TimeValidity? TimeValidity { get; set; }
    public DateTimeOffset? From { get; set; }
    public DateTimeOffset? To { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}