using AutoMapper;
using Domain.Dto;
using Domain.Manager.Interface;
using Domain.Model;
using Domain.Repository;
using Domain.Validator.Interface;
using System.Data.SqlClient;
using System.Net;

namespace Domain.Manager;

public class VoucherManager : IVoucherManager
{
    private readonly IMapper _mapper;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IRedeemValidator _redeemValidator;
    private readonly Locker _locker;

    public VoucherManager(IVoucherRepository voucherRepository, IRedeemValidator redeemValidator, IMapper mapper, Locker locker)
    {
        _voucherRepository = voucherRepository ?? throw new ArgumentNullException(nameof(voucherRepository));
        _redeemValidator = redeemValidator ?? throw new ArgumentNullException(nameof(redeemValidator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    }

    public async Task<MultipleResponseDto<VoucherDto>> Create(List<VoucherRequestDto> voucherDtos, CancellationToken ct)
    {
        if (voucherDtos == null) throw new ArgumentNullException(nameof(voucherDtos));

        var requests = _mapper.Map<IEnumerable<Voucher>>(voucherDtos);
        var result = new MultipleResponseDto<VoucherDto>();
        foreach (var request in requests)
        {
            if (ct.IsCancellationRequested)
            {
                break;
            }

            try
            {
                result.Ok(_mapper.Map<VoucherDto>(await _voucherRepository.Create(request, ct)));
            }
            catch (SqlException ex)
            {
                result.Error(new ErrorDto { Message = ex.Message });
            }
        }

        await _voucherRepository.Save(ct);
        return result;
    }

    public async Task<SingleResponseDto<RedeemResponseDto>> Redeem(Guid voucherId, CancellationToken ct)
    {
        await _locker.SemaphoreSlim.WaitAsync(ct);

        try
        {
            var voucher = _voucherRepository.GetVoucher(voucherId);

            if (voucher == null)
            {
                return new SingleResponseDto<RedeemResponseDto>
                {
                    Data = new RedeemResponseDto
                    {
                        IsSuccess = false
                    },
                    Error = new ErrorDto {Message = $"Voucher not found with id: {voucherId.ToString()}"},
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var validationResult = _redeemValidator.IsValidRedeem(voucher);

            if (!validationResult.IsSuccess)
            {
                return new SingleResponseDto<RedeemResponseDto>
                {
                    Data = new RedeemResponseDto
                    {
                        IsSuccess = false
                    },
                    Error = new ErrorDto { Message = validationResult.ErrorMessage },
                    StatusCode = HttpStatusCode.Conflict
                };
            }

            var redeem = new Redemption
            {
                VoucherId = voucher.Id,
                Voucher = voucher
            };
            await _voucherRepository.Redeem(redeem, ct);
            await _voucherRepository.Save(ct);
        }
        finally
        {
            _locker.SemaphoreSlim.Release();
        }

        return new SingleResponseDto<RedeemResponseDto>
        {
            Data = new RedeemResponseDto
            {
                IsSuccess = true
            },
            StatusCode = HttpStatusCode.OK
        };
    }
}

