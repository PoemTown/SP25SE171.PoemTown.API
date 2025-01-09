﻿using Microsoft.AspNetCore.Http;

namespace PoemTown.Service.BusinessModels.ResponseModels.PaginationResponses;

public class BasePaginationResponse<T>
{
    public int StatusCode { get; set; } = StatusCodes.Status200OK;
    public string? Message { get; set; } = "Successful";
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int CurrentPageRecords { get; set; }
    public IList<T> Data { get; set; }

    public BasePaginationResponse()
    {
        
    }
    public BasePaginationResponse(int statusCode, string? message, IList<T> data, int pageNumber, int pageSize, int totalRecords, int currentPageRecords)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        CurrentPageRecords = currentPageRecords;
    }
}