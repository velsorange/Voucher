using Domain.Dto;

namespace Domain.Manager.Interface;

public interface IVoucherManager
{
    Task<MultipleResponseDto<VoucherDto>> Create(List<VoucherRequestDto> voucherDtos, CancellationToken ct);
    Task<SingleResponseDto<RedeemResponseDto>> Redeem(Guid voucherId, CancellationToken ct);
}
