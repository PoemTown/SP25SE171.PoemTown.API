using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.BusinessModels.ResponseModels.Base;
using PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;
using PoemTown.Service.BusinessModels.ResponseModels.PoemResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.PoemFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.PoemSorts;

namespace PoemTown.API.Controllers;

public class PoemsController : BaseController
{
    private readonly IPoemService _poemService;
    private readonly IMapper _mapper;
    public PoemsController(IPoemService poemService, IMapper mapper)
    {
        _poemService = poemService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("v1")]
    [Authorize]
    public async Task<ActionResult<BaseResponse>> CreateNewPoem(CreateNewPoemRequest request)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        await _poemService.CreateNewPoem(userId, request);
        return Ok(new BaseResponse(StatusCodes.Status201Created, "Poem created successfully"));
    }
    
    [HttpGet]
    [Route("v1/mine")]
    [Authorize]
    public async Task<ActionResult<BasePaginationResponse<GetPoemResponse>>> GetMyPoems(RequestOptionsBase<GetMyPoemFilterOption, GetMyPoemSortOption> reqeust)
    {
        Guid userId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "UserId")!.Value);
        var paginationResponse = await _poemService.GetMyPoems(userId, reqeust);
        
        var basePaginationResponse = _mapper.Map<BasePaginationResponse<GetPoemResponse>>(paginationResponse);
        basePaginationResponse.StatusCode = StatusCodes.Status200OK;
        basePaginationResponse.Message = "Get poems successfully";
        
        return Ok(basePaginationResponse);
    }
}