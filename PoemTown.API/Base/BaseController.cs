using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoemTown.API.CustomAttribute;

namespace PoemTown.API.Base;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(ValidateModelStateAttribute))]
public class BaseController : ControllerBase
{
}