using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationIntegration.API.ApiResponse;
using NotificationIntegration.Domain.Services.Interfaces;
using System.Threading.Tasks;

namespace NotificationIntegration.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : BaseController
    {
        private INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [AllowAnonymous]
        [HttpPost, Route("send")]
        [ProducesResponseType(typeof(ApiBaseResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> SendMessage([Required] string message)
        {
            var response = await _notificationService.SendMessage(message);
            return Ok(response);
        }
    }
}
