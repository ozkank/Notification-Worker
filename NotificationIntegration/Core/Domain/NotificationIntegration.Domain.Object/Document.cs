using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace NotificationIntegration.Domain.Object
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}
