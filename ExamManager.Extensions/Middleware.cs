using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace ExamManager.Extensions
{
    public class Error
    {
        public int status { get; set; }
        public string? Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    public class Middleware : IMiddleware
    {

        private readonly ILogger<Middleware> _logger;

        public Middleware(ILogger<Middleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                context.Response.ContentType = "application/json";
                await next(context);
            }
            catch (Exception e)
            {
                
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                Error error = new Error
                {
                    status = context.Response.StatusCode,
                    Message = e.Message
                };

                await context.Response.WriteAsync(error.ToString());
            }

        }
    }
}
