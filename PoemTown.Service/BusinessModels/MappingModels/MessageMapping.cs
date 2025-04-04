using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.AccountResponses;
using PoemTown.Service.BusinessModels.ResponseModels.ChatResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemTown.Service.BusinessModels.MappingModels
{
    public class MessageMapping : Profile
    {
        public MessageMapping()
        {
            CreateMap<Message, GetMesssageWithPartner>()
                    .ForMember(dest => dest.FromUser, opt => opt.MapFrom(src => src.FromUser))
                    .ForMember(dest => dest.ToUser, opt => opt.MapFrom(src => src.ToUser))
                    .ReverseMap();
            CreateMap<User, GetChatPartner>().ReverseMap();

        }
    }
}
