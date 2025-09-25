using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using University.Core.Exceptions;

namespace University.API.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                _logger.LogWarning(context.Exception, "Item not Found");
                context.Result = Response(context.Exception.Message, "Item not Found", StatusCodes.Status404NotFound);
                return;
            }

            if (context.Exception is BusinessException businessException)
            {
                if (businessException.Errors.Any())
                    context.Result = Response(businessException.Errors, "One or more business validation errors", StatusCodes.Status400BadRequest);
                else
                    context.Result = Response(businessException.Message, "One or more business validation errors", StatusCodes.Status400BadRequest);
                return;
            }

            if (context.Exception is ArgumentException)
            {
                context.Result = Response(context.Exception.Message, "Missing data", StatusCodes.Status400BadRequest);
                return;
            }

            if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = Response(context.Exception.Message, "Unauthorized", StatusCodes.Status403Forbidden);
                return;
            }

            _logger.LogError(context.Exception, context.Exception.Message);
            context.Result = Response(context.Exception.Message, "Internal Server Error", StatusCodes.Status500InternalServerError, context.Exception.StackTrace);
        }

        public ObjectResult Response(string message, string title, int status, string? stackTrace = null)
        {
            var result = new ApiResponse
            {
                StatusCode = status,
                Message = message,
                ResponseException = title,
                IsError = true,
                Version = "1.0",
                Result = stackTrace
            };
            return new ObjectResult(result) { StatusCode = status };
        }

        public ObjectResult Response(Dictionary<string, List<string>> errors, string title, int status)
        {
            var response = new ApiResponse
            {
                StatusCode = status,
                Message = title,
                ResponseException = title,
                IsError = true,
                Version = "1.0",
                Result = errors
            };
            return new ObjectResult(response) { StatusCode = status };
        }
    }
}
