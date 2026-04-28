using Domain.Exceptions;
using Shared.ErrorModels;

namespace E_Commerce.API.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                    await HandleNotFoundApiAsync(context);
            }
            catch (Exception ex) {
                _logger.LogError($"Somthing went wrong {ex.Message}");
                await HandleExceptionAsync(context,ex);
            }
        }

        private async Task HandleNotFoundApiAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails() { 
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"The EndPoint With URL {context.Request.Path} Not Found"
            }.ToString();
            await context.Response.WriteAsync(response);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // 1] change status code
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                (_) => StatusCodes.Status500InternalServerError
            };

            // 2] change content type
            context.Response.ContentType = "application/json";

            // 3] write password in body
            var response = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = ex.Message
            }.ToString();
            await context.Response.WriteAsync(response);
        }
    }
}
