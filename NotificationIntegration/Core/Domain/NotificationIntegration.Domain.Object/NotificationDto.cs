namespace NotificationIntegration.Domain.Object
{
    [BsonCollection("messages")]
    public class NotificationDto : Document
    {
        public string Message { get; set; }
    }
}
