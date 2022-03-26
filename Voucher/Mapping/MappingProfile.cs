using AutoMapper;
using Domain.Dto;
using Domain.Model;

namespace VoucherApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VoucherRequestDto, Voucher>();
            CreateMap<Voucher, VoucherDto>();
        }
    }
}
