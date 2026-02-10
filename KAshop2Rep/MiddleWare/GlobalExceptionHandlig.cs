using DAL.DTO.Response;

namespace KAshop2Rep.MiddleWare
{
    public class GlobalExceptionHandlig
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlig(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                var errorDetails = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "server error",
                    StackTrace=e.InnerException.Message
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }
    }
}
