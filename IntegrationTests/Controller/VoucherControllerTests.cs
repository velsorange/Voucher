using System;
using System.Collections.Generic;
using System.Net;
using IntegrationTests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Bogus;
using Domain.Dto;
using Domain.Model;
using FluentAssertions;

namespace IntegrationTests.Controller;


[TestClass]
public class VoucherControllerTests : WebHostedTestBase
{
    private string _voucherUrl = "voucher";
    private string _redeemUrl = "voucher/redeem";

    [TestMethod]
    public async Task GivenVoucherRequest_WhenAdding_ThenNewVoucherShouldBeReceived()
    {
        // Arrange
        var faker = new Faker();
        var voucherType = (VoucherType)faker.Random.Int(0, 3);
        var dto = new VoucherRequestDto()
        {
            Type = voucherType,
            MaxRedemptionCount = voucherType == VoucherType.XTimes ? faker.Random.Int(10) : null,
            TimeValidity = TimeValidity.From,
            From = DateTimeOffset.UtcNow
        };
        var requests = new List<VoucherRequestDto>
        {
            dto
        };

        // Act

        var responses = await Client.PostAsync<MultipleResponseDto<VoucherDto>>($"{_voucherUrl}", requests.MakeStringContent());

        // Assert
        responses.Errors.Should().BeEmpty();
        responses.Objects.Should().BeEquivalentTo(requests);
    }

    [TestMethod]
    public async Task GivenWrongVoucherRequest_WhenAdding_ThenErrorShouldBeReceived()
    {
        // Arrange
        var dto = new VoucherRequestDto()
        {
            Type = VoucherType.XTimes
        };
        var requests = new List<VoucherRequestDto>
        {
            dto
        };

        // Act

        var responses = await Client.PostAsync($"{_voucherUrl}", requests.MakeStringContent());

        // Assert
        responses.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await responses.Content.ReadAsStringAsync()).Should().Contain("XTimes requires MaxRedemptionCount.");
    }


    [TestMethod]
    public async Task GivenMultipleVoucherRequest_WhenAdding_ThenNewVoucherShouldBeReceived()
    {
        // Arrange
        var faker = new Faker();
        var voucherType = (VoucherType)faker.Random.Int(0, 3);
        var dto1 = new VoucherRequestDto()
        {
            Type = voucherType,
            MaxRedemptionCount = voucherType == VoucherType.XTimes ? faker.Random.Int(10) : null,
            TimeValidity = TimeValidity.From,
            From = DateTimeOffset.UtcNow
        };
        voucherType = (VoucherType)faker.Random.Int(0, 3);
        var dto2 = new VoucherRequestDto()
        {
            Type = voucherType,
            MaxRedemptionCount = voucherType == VoucherType.XTimes ? faker.Random.Int(10) : null,
            TimeValidity = TimeValidity.From,
            From = DateTimeOffset.UtcNow
        };
        voucherType = (VoucherType)faker.Random.Int(0, 3);
        var dto3 = new VoucherRequestDto()
        {
            Type = voucherType,
            MaxRedemptionCount = voucherType == VoucherType.XTimes ? faker.Random.Int(10) : null,
            TimeValidity = TimeValidity.From,
            From = DateTimeOffset.UtcNow
        };
        var requests = new List<VoucherRequestDto>
        {
            dto1, dto2, dto3
        };

        // Act

        var responses = await Client.PostAsync<MultipleResponseDto<VoucherDto>>($"{_voucherUrl}", requests.MakeStringContent());

        // Assert
        responses.Errors.Should().BeEmpty();
        responses.Objects.Should().BeEquivalentTo(requests);
    }

    [TestMethod]
    public async Task GivenVoucher_WhenRedeemAValidMultipleVoucher_ThenSuccessResponseShouldBeReceived()
    {
        // Arrange
        var dto = new VoucherRequestDto()
        {
            Type = VoucherType.Multiple
        };
        var requests = new List<VoucherRequestDto>
        {
            dto
        };
        var createResponse = await Client.PostAsync<MultipleResponseDto<VoucherDto>>($"{_voucherUrl}", requests.MakeStringContent());

        // Act
        var redeemResponse = await Client.PostAsync<SingleResponseDto<RedeemResponseDto>>($"{_redeemUrl}/{createResponse.Objects[0].Id}");

        // Assert
        redeemResponse.Error.Should().BeNull();
        redeemResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        redeemResponse.Data.IsSuccess.Should().BeTrue();
    }

    [TestMethod]
    public async Task GivenVoucher_WhenRedeemAValidVoucher_ThenSuccessResponseShouldBeReceived()
    {
        // Arrange
        var dto = new VoucherRequestDto()
        {
            Type = VoucherType.Single,
            TimeValidity = TimeValidity.From,
            From = DateTimeOffset.UtcNow.AddDays(-1)
        };
        var requests = new List<VoucherRequestDto>
        {
            dto
        };
        var createResponse = await Client.PostAsync<MultipleResponseDto<VoucherDto>>($"{_voucherUrl}", requests.MakeStringContent());

        // Act
        var redeemResponse = await Client.PostAsync<SingleResponseDto<RedeemResponseDto>>($"{_redeemUrl}/{createResponse.Objects[0].Id}");

        // Assert
        redeemResponse.Error.Should().BeNull();
        redeemResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        redeemResponse.Data.IsSuccess.Should().BeTrue();
    }

    [TestMethod]
    public async Task GivenVoucher_WhenRedeemAnInvalidVoucher_ThenErrorResponseShouldBeReceived()
    {
        // Arrange
        var dto = new VoucherRequestDto()
        {
            Type = VoucherType.Single,
            TimeValidity = TimeValidity.From,
            From = DateTimeOffset.UtcNow.AddDays(1)
        };
        var requests = new List<VoucherRequestDto>
        {
            dto
        };
        var createResponse = await Client.PostAsync<MultipleResponseDto<VoucherDto>>($"{_voucherUrl}", requests.MakeStringContent());

        // Act
        var redeemResponse = await Client.PostAsync($"{_redeemUrl}/{createResponse.Objects[0].Id}", null);

        // Assert
        redeemResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var responseObject = await redeemResponse.GetContentAsAsync<SingleResponseDto<RedeemResponseDto>>();
        responseObject.StatusCode.Should().Be(HttpStatusCode.Conflict);
        responseObject.Error.Should().NotBeNull();
        responseObject.Error.Message.Should().Be("Voucher is not valid yet.");
        responseObject.Data.IsSuccess.Should().BeFalse();
    }
}