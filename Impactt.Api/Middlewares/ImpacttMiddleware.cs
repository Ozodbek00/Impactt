using Impactt.Service.Exceptions;

namespace Impactt.Api.Middlewares
{
    public sealed class ImpacttMiddleware
    {
        private readonly RequestDelegate _next;

        public ImpacttMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ImpacttException exception)
            {
                await ClientErrorHandleAsync(httpContext, exception);
            }
            catch (Exception exception)
            {
                await SystemErrorHandleAsync(httpContext, exception);
            }
        }

        public async Task ClientErrorHandleAsync(HttpContext httpContext, ImpacttException exception)
        {
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = exception.Code;

            await httpContext.Response.WriteAsync(exception.Message);
        }

        public async Task SystemErrorHandleAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 500;

            await httpContext.Response.WriteAsync(exception.Message);
        }
    }
}
