using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NotificationIntegration.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public class BaseController : ControllerBase
    {
    }
}
