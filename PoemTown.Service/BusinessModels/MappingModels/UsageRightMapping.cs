using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.UsageResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.MappingModels
{
    public class UsageRightMapping : Profile
    {
        public UsageRightMapping()
        {
            CreateMap<UsageRight, GetSoldPoemResponse>()
                .ForMember(dest => dest.Poem, opt => opt.MapFrom(p => p.SaleVersion.Poem));

            CreateMap<UsageRight, GetBoughtPoemResponse>();
            CreateMap<UsageRight, GetUserBoughtUsage>()
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(u => u.User));

        }
    }
}
