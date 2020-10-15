using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationIntegration.API.ApiResponse;
using NotificationIntegration.Domain.Services.Interfaces;
using System.Threading.Tasks;
using NotificationIntegration.Domain.Object;

namespace NotificationIntegration.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : BaseController
    {
        private INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService)
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

        [AllowAnonymous]
        [HttpGet, Route("")]
        [ProducesResponseType(typeof(ApiBaseResponse<List<NotificationDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<NotificationDto>>> GetList()
        {
            var response = _notificationService.GetMessages();
            return Ok(response);
        }
    }
}
