using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardDetailResponses;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardResponses;
using PoemTown.Service.BusinessModels.ResponseModels.UserLeaderBoardResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.MappingModels
{
    public class LeaderboardMapping : Profile
    {
        public LeaderboardMapping() 
        { 
            CreateMap<LeaderBoard, GetLeaderBoardResponse>();

            CreateMap<LeaderBoardDetail, GetLeaderBoardDetailResponse>()
                .ForMember(dest => dest.Poem, opt => opt.MapFrom(src => src.Poem));

            CreateMap<UserLeaderBoard, GetUserLeaderBoardResponse>();
        }
    }
}
