using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NotificationIntegration.API.ApiResponse;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NotificationIntegration.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string requestBody = string.Empty;
            try
            {
                httpContext.Request.EnableBuffering();

                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, false, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    httpContext.Request.Body.Position = 0;
                    await next(httpContext);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Message :{ex.Message} - Request:{requestBody}");
                if (httpContext.Response.HasStarted)
                {
                    throw;
                }

                httpContext.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new ApiBaseResponse<object>()
                {
                    Data = null,
                    Error = ex.Message
                }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                await httpContext.Response.WriteAsync(result);

            }
        }
    }
}
