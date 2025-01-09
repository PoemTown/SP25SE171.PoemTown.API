using Microsoft.AspNetCore.Http;

namespace PoemTown.Service.BusinessModels.ResponseModels.Base;

public class BaseResponse<T>
{
    public int StatusCode { get; set; } = StatusCodes.Status200OK;
    public string? Message { get; set; } = "Successful";
    public T? Data { get; set; }

    public BaseResponse(int statusCode, string? message, T? data)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }
    public BaseResponse()
    {
    }

}

public class BaseResponse : BaseResponse<object>
{
    public BaseResponse(int statusCode, string? message, object? data) : base(statusCode, message, data)
    {
    }
    public BaseResponse(int statusCode, string? message) : base(statusCode, message, null)
    {
    }
}