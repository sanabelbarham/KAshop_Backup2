namespace KAshop2Rep.MiddleWare
{
    public class CustomMiddleWare
    {
        private readonly RequestDelegate _next;

        public CustomMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
          
                Console.WriteLine("processing request ");
                await _next(context);
                Console.WriteLine("processing responce ");
       
           
                Console.WriteLine("hhhh");
          
        }
    }
}
