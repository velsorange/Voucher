using System;
using System.Linq;
using Domain.Dto;
using Domain.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ValidatorTests;

[TestClass]
public class VoucherRequestTests
{

    [TestMethod]
    public void GivenXTimesVoucherRequestDtoWithoutMaxRedemptionCount_WhenValidating_ThanProperValidationResultShouldBeReceived()
    {
        // Arrange
        var request = new VoucherRequestDto
        {
            Type = VoucherType.XTimes
        };

        // Act
        var result = request.Validate(null);

        // Assert
        result.Should().NotBeNull();
        var resultList = result.ToList();
        resultList.Count.Should().Be(1);
        resultList[0].ErrorMessage.Should().Be("XTimes requires MaxRedemptionCount.");
    }


    [TestMethod]
    public void GivenFromVoucherRequestDtoWithoutFrom_WhenValidating_ThanProperValidationResultShouldBeReceived()
    {
        // Arrange
        var request = new VoucherRequestDto
        {
            Type = VoucherType.Multiple,
            TimeValidity = TimeValidity.From
        };

        // Act
        var result = request.Validate(null);

        // Assert
        result.Should().NotBeNull();
        var resultList = result.ToList();
        resultList.Count.Should().Be(1);
        resultList[0].ErrorMessage.Should().Be("TimeValidity.From requires From.");
    }

    [TestMethod]
    public void GivenToVoucherRequestDtoWithoutTo_WhenValidating_ThanProperValidationResultShouldBeReceived()
    {
        // Arrange
        var request = new VoucherRequestDto
        {
            Type = VoucherType.Multiple,
            TimeValidity = TimeValidity.To
        };

        // Act
        var result = request.Validate(null);

        // Assert
        result.Should().NotBeNull();
        var resultList = result.ToList();
        resultList.Count.Should().Be(1);
        resultList[0].ErrorMessage.Should().Be("TimeValidity.To requires To.");
    }

    [TestMethod]
    public void GivenFromToVoucherRequestDtoWithoutFromAndTo_WhenValidating_ThanProperValidationResultShouldBeReceived()
    {
        // Arrange
        var request = new VoucherRequestDto
        {
            Type = VoucherType.Multiple,
            TimeValidity = TimeValidity.FromTo
        };

        // Act
        var result = request.Validate(null);

        // Assert
        result.Should().NotBeNull();
        var resultList = result.ToList();
        resultList.Count.Should().Be(2);
        resultList[0].ErrorMessage.Should().Be("TimeValidity.FromTo requires From.");
        resultList[1].ErrorMessage.Should().Be("TimeValidity.FromTo requires To.");
    }

    [TestMethod]
    public void GivenFromToVoucherRequestDtoWithoutFrom_WhenValidating_ThanProperValidationResultShouldBeReceived()
    {
        // Arrange
        var request = new VoucherRequestDto
        {
            Type = VoucherType.Multiple,
            TimeValidity = TimeValidity.FromTo,
            To = DateTimeOffset.UtcNow.AddDays(1)
        };

        // Act
        var result = request.Validate(null);

        // Assert
        result.Should().NotBeNull();
        var resultList = result.ToList();
        resultList.Count.Should().Be(1);
        resultList[0].ErrorMessage.Should().Be("TimeValidity.FromTo requires From.");
    }

    [TestMethod]
    public void GivenFromToVoucherRequestDtoWithoutTo_WhenValidating_ThanProperValidationResultShouldBeReceived()
    {
        // Arrange
        var request = new VoucherRequestDto
        {
            Type = VoucherType.Multiple,
            TimeValidity = TimeValidity.FromTo,
            From = DateTimeOffset.UtcNow.AddDays(1)
        };

        // Act
        var result = request.Validate(null);

        // Assert
        result.Should().NotBeNull();
        var resultList = result.ToList();
        resultList.Count.Should().Be(1);
        resultList[0].ErrorMessage.Should().Be("TimeValidity.FromTo requires To.");
    }

    [TestMethod]
    public void GivenFromToVoucherRequestDtoWithWronDates_WhenValidating_ThanProperValidationResultShouldBeReceived()
    {
        // Arrange
        var request = new VoucherRequestDto
        {
            Type = VoucherType.Multiple,
            TimeValidity = TimeValidity.FromTo,
            From = DateTimeOffset.UtcNow.AddDays(1),
            To = DateTimeOffset.UtcNow.AddDays(-1)
        };

        // Act
        var result = request.Validate(null);

        // Assert
        result.Should().NotBeNull();
        var resultList = result.ToList();
        resultList.Count.Should().Be(1);
        resultList[0].ErrorMessage.Should().Be("TimeValidity.FromTo requires earlier From than To.");
    }
}