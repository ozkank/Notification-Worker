using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationIntegration.Infrastructure.Repositories
{
    [BsonCollection("messages")]
    public class NotificationDto : Document
    {
        public string Message { get; set; }
    }
}
