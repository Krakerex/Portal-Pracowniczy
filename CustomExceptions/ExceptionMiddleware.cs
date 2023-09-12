namespace krzysztofb.Services
{
    using krzysztofb.CustomExceptions;
    using Microsoft.EntityFrameworkCore;
    using System.Net;
    using System.Text.Json;



    namespace Request.Exceptions
    {
        /// <summary>
        /// Middleware do obsługi wyjątków
        /// </summary>
        public class ExceptionMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<ExceptionMiddleware> _logger;
            public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }



            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context); // Continue processing the request
                }
                catch (Exception ex)
                {

                    await HandleExceptionAsync(context, ex);
                }
            }



            private async Task HandleExceptionAsync(HttpContext context, Exception exception)
            {
                context.Response.ContentType = "application/json";
                string message;
                if (exception is NullReferenceException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    message = "Not Found";
                    _logger.LogError(exception.Message, message, DateTime.UtcNow);
                }
                else if (exception is BadHttpRequestException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = "Bad request";
                    _logger.LogError(exception.Message, message, DateTime.UtcNow);
                }
                else if (exception is DbUpdateException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = "Bad request";
                    _logger.LogError(exception.Message, message, DateTime.UtcNow);
                }
                else if (exception is PdfToDatabaseException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    message = "Form invalid";
                    _logger.LogError(exception.Message, message, DateTime.UtcNow);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = "Internal server error";
                    _logger.LogError(exception.Message, message, DateTime.UtcNow);
                }


                var errorResponse = new
                {
                    context.Response.StatusCode,
                    Message = message,
                    Time = DateTime.UtcNow

                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }
}