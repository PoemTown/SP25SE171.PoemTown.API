using AutoMapper;
using PoemTown.Repository.Entities;
using PoemTown.Service.BusinessModels.ResponseModels.CommentResponses;

namespace PoemTown.Service.BusinessModels.MappingModels;

public class CommentMapping : Profile
{
    public CommentMapping()
    {
        CreateMap<Comment, GetCommentResponse>().ReverseMap();
    }
}