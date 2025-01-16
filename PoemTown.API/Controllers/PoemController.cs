using Microsoft.AspNetCore.Mvc;
using PoemTown.API.Base;
using PoemTown.Service.BusinessModels.RequestModels.PoemRequests;
using PoemTown.Service.Interfaces;

namespace PoemTown.API.Controllers;

public class PoemController : BaseController
{
    private readonly IPoemService _poemService;
    public PoemController(IPoemService poemService)
    {
        _poemService = poemService;
    }

    [HttpPost]
    [Route("v1")]
    public async Task CreateNewPoem(CreateNewPoemRequest request)
    {
        
    }
}