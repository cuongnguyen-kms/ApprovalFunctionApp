using ApprovalFunctionApp.DTOs;
using ApprovalFunctionApp.Models;
using AutoMapper;
using System;

namespace ApprovalFunctionApp.Mapping
{
    public class ApprovalMappingProfile : Profile
    {
        public ApprovalMappingProfile()
        {
            CreateMap<ApprovalRequestDto, ApprovalRequest>()
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
