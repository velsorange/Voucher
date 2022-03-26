using System;
using System.Collections.Generic;
using Domain.Model;
using Domain.Validator;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ValidatorTests;

[TestClass]
public class ValidatorTests
{
    [TestMethod]
    [DataRow(VoucherType.Single, typeof(SingleRedeemValidator))]
    [DataRow(VoucherType.Multiple, typeof(MultipleRedeemValidator))]
    [DataRow(VoucherType.XTimes, typeof(XTimesRedeemValidator))]
    public void GivenTypeRedeemValidatorFactory_WhenGettinValidatorByType_ThanProperInstanceShouldBeReceived(VoucherType voucherType, Type expectedType)
    {
        // Arrange
        var factory = new TypeRedeemValidatorFactory();

        // Act
        var validator = factory.GetValidator(voucherType);

        // Assert
        validator.GetType().Should().Be(expectedType);
    }

    [TestMethod]
    public void GivenSingleRedeemValidator_WhenValidationWithoutRedemptions_ThanSuccessShouldBeReceived()
    {
        // Arrange
        var voucher = new Voucher
        {
            Redemptions = new List<Redemption>()
        };
        var validator = new SingleRedeemValidator();

        // Act
        var result = validator.IsValidRedeem(voucher);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [TestMethod]
    public void GivenSingleRedeemValidator_WhenValidationWithRedemptions_ThanErrorShouldBeReceived()
    {
        // Arrange
        var voucher = new Voucher
        {
            Redemptions = new List<Redemption>{ new() }
        };
        var validator = new SingleRedeemValidator();

        // Act
        var result = validator.IsValidRedeem(voucher);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Voucher has already redeemed.");
    }

    [TestMethod]
    public void GivenMultipleRedeemValidator_WhenValidate_ThanSuccessShouldBeReceived()
    {
        // Arrange
        var voucher = new Voucher();
        var validator = new MultipleRedeemValidator();

        // Act
        var result = validator.IsValidRedeem(voucher);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [TestMethod]
    public void GivenXTimesRedeemValidator_WhenValidationWithoutRedemptions_ThanSuccessShouldBeReceived()
    {
        // Arrange
        var voucher = new Voucher
        {
            Redemptions = new List<Redemption>(),
            MaxRedemptionCount = 2
        };
        var validator = new XTimesRedeemValidator();

        // Act
        var result = validator.IsValidRedeem(voucher);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [TestMethod]
    public void GivenXTimesRedeemValidator_WhenValidationWithEqualRedemptions_ThanErrorShouldBeReceived()
    {
        // Arrange
        var voucher = new Voucher
        {
            Redemptions = new List<Redemption> { new() },
            MaxRedemptionCount = 1
        };
        var validator = new XTimesRedeemValidator();

        // Act
        var result = validator.IsValidRedeem(voucher);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("The voucher is redeemed in the authorized quantity.");
    }

    [TestMethod]
    public void GivenXTimesRedeemValidator_WhenValidationWithHigherRedemptions_ThanErrorShouldBeReceived()
    {
        // Arrange
        var voucher = new Voucher
        {
            Redemptions = new List<Redemption> { new(), new() },
            MaxRedemptionCount = 1
        };
        var validator = new XTimesRedeemValidator();

        // Act
        var result = validator.IsValidRedeem(voucher);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("The voucher is redeemed in the authorized quantity.");
    }

    [TestMethod]
    [DataRow(TimeValidity.From, -1, 0, true, "" )]
    [DataRow(TimeValidity.From, 1, 0, false, "Voucher is not valid yet.")]
    [DataRow(TimeValidity.To, 0, 1, true, "")]
    [DataRow(TimeValidity.To, 0, -1, false, "The voucher is no longer valid.")]
    [DataRow(TimeValidity.FromTo, -1, 1, true, "")]
    [DataRow(TimeValidity.FromTo, 1, 2, false, "Voucher is not valid.")]
    [DataRow(TimeValidity.FromTo, -2, -1, false, "Voucher is not valid.")]
    public void GivenTimeRedeemValidator_WhenValidation_ThanProperResponseShouldBeReceived(TimeValidity timeValidity, 
        int fromAddDays,
        int toAddDays,
        bool expectedSuccess, 
        string expectedMessage)
    {
        // Arrange
        var validator = new TimeRedeemValidator();
        var now = DateTimeOffset.UtcNow;
        var voucher = new Voucher
        {
            TimeValidity = timeValidity,
            From = now.AddDays(fromAddDays),
            To = now.AddDays(toAddDays)
        };

        // Act
        var result = validator.IsValidRedeem(voucher);

        // Assert
        result.IsSuccess.Should().Be(expectedSuccess);
        if (!expectedSuccess)
        {
            result.ErrorMessage.Should().Be(expectedMessage);
        }
    }
}