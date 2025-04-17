using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Poems;

namespace PoemTown.Service.BusinessModels.RequestModels.PoemRequests;

public class PoemAiChatCompletionRequest
{
    public Guid? PoemTypeId { get; set; }
    public string PoemContent { get; set; }
    public string ChatContent { get; set; }
    public int? MaxToken { get; set; } = 100;
}