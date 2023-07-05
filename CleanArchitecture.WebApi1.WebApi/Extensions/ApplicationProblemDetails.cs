using CleanArchitecture.WebApi1.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CleanArchitecture.WebApi1.WebApi.Extensions
{
    public class ApplicationProblemDetails :ProblemDetails
    {
        public string Message { get; set; }
        public ApplicationProblemDetails(ApiException ex,string traceId)
        {
            Title = "Request validation error";
            Status = StatusCodes.Status400BadRequest;
            Type = "https://httpstatuses.com/400";
            Message = ex.Message.ToString();
            Instance = traceId;
        }
    }
}
