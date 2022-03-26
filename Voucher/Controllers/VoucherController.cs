using Domain.Dto;
using Domain.Manager.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace VoucherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VoucherController : ControllerBase
{
    private readonly IVoucherManager _voucherManager;


    public VoucherController(IVoucherManager voucherManager)
    {
        _voucherManager = voucherManager;
    }

    /// <summary>
    /// Create set of vouchers
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MultipleResponseDto<VoucherDto>>> Create([FromBody] List<VoucherRequestDto> vouchers, CancellationToken ct)
    {
        var result = await _voucherManager.Create(vouchers, ct);
        return Ok(result);
    }

    /// <summary>
    /// Redeem of voucher
    /// </summary>
    /// <returns></returns>
    [HttpPost("redeem/{voucherId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SingleResponseDto<RedeemResponseDto>>> Redeem(Guid voucherId, CancellationToken ct)
    {
        var result = await _voucherManager.Redeem(voucherId, ct);
        return GetResult(result);
    }

    private ActionResult<SingleResponseDto<RedeemResponseDto>> GetResult(SingleResponseDto<RedeemResponseDto> response)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.OK => Ok(response),
            HttpStatusCode.NotFound => NotFound(response),
            HttpStatusCode.Conflict => Conflict(response),
            _ => BadRequest()
        };
    }
}
