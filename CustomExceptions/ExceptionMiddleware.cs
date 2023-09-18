namespace krzysztofb.Services
{
    using krzysztofb.CustomExceptions;
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
                switch (exception)
                {
                    case DatabaseValidationException:
                        context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        message = "Database validation failed on given data";
                        _logger.LogError(exception.Message, message, DateTime.UtcNow);
                        break;
                    case PdfToDatabaseException:
                        context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                        message = "Data validation failed on given form";
                        _logger.LogError(exception.Message, message, DateTime.UtcNow);
                        break;
                    case UploadException:
                        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                        message = "File missing or invalid";
                        _logger.LogError(exception.Message, message, DateTime.UtcNow);
                        break;
                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        message = "Internal server error";
                        _logger.LogError(exception.Message, message, DateTime.UtcNow);
                        break;
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