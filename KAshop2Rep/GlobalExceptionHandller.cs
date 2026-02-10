using DAL.DTO.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace KAshop2Rep
{
    public class GlobalExceptionHandller : IExceptionHandler
    {
        public async  ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {

            var errorDetails = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "server error",
         
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(errorDetails);
            return true;
        }
    }
}
