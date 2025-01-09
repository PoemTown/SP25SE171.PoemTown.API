﻿using System.Text.Json;
using PoemTown.Repository.CustomException;

namespace PoemTown.API.CustomMiddleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
    public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CoreException ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.StatusCode = ex.StatusCode;
            var result = JsonSerializer.Serialize(new
            {
                statusCode = ex.StatusCode,
                errorMessage = ex.ErrorMessage
            });
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var result = JsonSerializer.Serialize(new
            {
                errorMessage = $"An unexpected error occurred. Detail: {ex.Message}"
            });
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
        
    }
}