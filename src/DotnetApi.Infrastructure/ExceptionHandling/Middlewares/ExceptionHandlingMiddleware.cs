﻿using System;
using DotnetApi.Infrastructure.ExceptionHandling.Extensions;
using DotnetApi.Infrastructure.ExceptionHandling.Services;
using Microsoft.AspNetCore.Http;

namespace DotnetApi.Infrastructure.ExceptionHandling.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IExceptionHandlerInvoker _exceptionHandlerInvoker;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        IExceptionHandlerInvoker exceptionHandlerInvoker)
    {
        _next = next;
        _exceptionHandlerInvoker = exceptionHandlerInvoker;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var problemDetails = _exceptionHandlerInvoker.Handle(ex);
            await context.SetProblemDetailsResponse(problemDetails);
        }
    }
}

