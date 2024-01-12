using System.Diagnostics.CodeAnalysis;

namespace PostService.Api.Infrastructure.Middlewares
{
    [ExcludeFromCodeCoverage]
    public class ErrorHandlingMiddleware
    {
        static readonly Serilog.ILogger Log = Serilog.Log.ForContext<ErrorHandlingMiddleware>();
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Exception exception = GetInnermostExceptionMessage(ex);
                Log.Fatal(exception, exception.Message);

                string message = "An error occurred. Please try again later.";

                if (exception != null)
                {
                    message = exception.Message;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "text/plain";
                }

                await context.Response.WriteAsJsonAsync(new { error = message });
            }
        }

        public static Exception? GetInnermostExceptionMessage(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            if (ex.InnerException == null)
            {
                return ex;
            }

            return GetInnermostExceptionMessage(ex.InnerException);
        }
    }
}
