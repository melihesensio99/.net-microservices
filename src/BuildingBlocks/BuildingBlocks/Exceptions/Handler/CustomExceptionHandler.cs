using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error Message : {ExceptionMessage} Time of occurence {time}", exception.Message, DateTime.UtcNow);
            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InternalServerException => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError),
                ValidationException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
                BadRequestException => (exception.Message, exception.GetType().Name, StatusCodes.Status400BadRequest),
                NotFoundException => (exception.Message, exception.GetType().Name, StatusCodes.Status404NotFound),
                _ => (exception.Message, exception.GetType().Name, StatusCodes.Status500InternalServerError)
            };
            httpContext.Response.StatusCode = details.StatusCode;

            var problemDetails = new ProblemDetails
            {
                Detail = details.Detail,
                Status = details.StatusCode,
                Title = details.Title,
                Instance = httpContext.Request.Path

            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if (exception is FluentValidation.ValidationException validationException)
            {
                problemDetails.Extensions.Add("Errors", validationException.Errors);
            }

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
