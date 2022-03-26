namespace Model;

public enum VoucherType
{
    Single,
    Multiple,
    XTimes
}

public enum TimeValidity
{
    From,
    FromTo,
    To
}

public class Voucher
{
    public Guid Id { get; set; }
    public VoucherType Type { get; set; }
    public int MaxRedemptionCount { get; set; }
    public TimeValidity TimeValidity { get; set; }
    public DateTimeOffset From { get; set; }
    public DateTimeOffset To { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public virtual List<Redemption> Redemptions { get; set; } = new List<Redemption>();
}

public class Redemption
{
    public Guid Id { get; set; }
    public Guid VoucherId { get; set; }
    public Voucher Voucher { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
