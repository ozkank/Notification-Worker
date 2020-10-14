using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NotificationIntegration.API.ApiResponse;

namespace NotificationIntegration.API.Filters
{
    public class ActionTrackingFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

            if (context.Result is OkObjectResult okObjectResult)
            {
                var newResponse = new ApiBaseResponse<object>
                {
                    Data = okObjectResult.Value
                };
                context.Result = new OkObjectResult(newResponse);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
