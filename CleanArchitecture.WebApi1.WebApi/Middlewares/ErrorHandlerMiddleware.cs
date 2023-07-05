using CleanArchitecture.WebApi1.Application.Exceptions;
using CleanArchitecture.WebApi1.Application.Wrappers;
using CleanArchitecture.WebApi1.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi1.WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

                switch (error)
                {
                    case Application.Exceptions.ApiException e:
                        // custom application error
                        var problemDetails = GetBadRequestProblemDetails(e);
                        response = context.Response;
                        response.ContentType = "application/json";
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                        break;
                    case ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        var result = JsonSerializer.Serialize(responseModel);

                        await response.WriteAsync(result);
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        var result1 = JsonSerializer.Serialize(responseModel);

                        await response.WriteAsync(result1);
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var result2 = JsonSerializer.Serialize(responseModel);

                        await response.WriteAsync(result2);
                        break;
                }
           
            }
           
        }
        private ApplicationProblemDetails GetBadRequestProblemDetails(ApiException ex)
        {
            string traceId = Guid.NewGuid().ToString();

            var invalidRequestProblemDetails = new ApplicationProblemDetails(ex, traceId);

            return invalidRequestProblemDetails;
        }
    }
}
