using Domain.Manager;
using Domain.Manager.Interface;
using Domain.Validator;
using Domain.Validator.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DomainService
{
    public static void AddDomainService(this IServiceCollection services)
    {
        services.AddScoped<IVoucherManager, VoucherManager>();
        services.AddScoped<IRedeemValidator, RedeemValidator>();
        services.AddScoped<ITimeRedeemValidator, TimeRedeemValidator>();
        services.AddScoped<ITypeRedeemValidatorFactory, TypeRedeemValidatorFactory>();
        services.AddSingleton<Locker>();
    }
}