using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.AchievementRespponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

namespace PoemTown.Service.BusinessModels.MappingModels
{
    public class AchievementMapping : Profile
    {
        public AchievementMapping()
        {
            CreateMap<Achievement, GetAchievementResponse>().ReverseMap();

            CreateMap<GetAchievementResponse, GetUserProfileResponse>();
        }
    }
}
