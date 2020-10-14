using System;
using MongoDB.Bson;

namespace  NotificationIntegration.Infrastructure.Repositories
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}
