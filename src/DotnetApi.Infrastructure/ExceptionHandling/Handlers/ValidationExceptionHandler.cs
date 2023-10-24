﻿using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Infrastructure.ExceptionHandling.Handlers;

public class ValidationExceptionHandler : IExceptionHandler<ValidationException>
{
    public ProblemDetails CreateProblemDetailsFromException(ValidationException exception)
    {
        var message = exception.Message;

        return new ProblemDetails
        {
            Title = "Bad Request",
            Status = StatusCodes.Status400BadRequest,
            Detail = message
        };
    }
}

