using System;
using System.Runtime.Serialization;

namespace TelegramHistory.Data
{
    [DataContract]
    public class MessageData
    {
        [DataMember(Name = "filename")]
        public string Filename { get; set; }

        [DataMember(Name = "fromUserId")]
        public int FromUserId { get; set; }

        [DataMember(Name = "messageId")]
        public int MessageId { get; set; }

        [DataMember(Name = "messageType")]
        public MessageType MessageType { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "timestamp")]
        public DateTimeOffset Timestamp { get; set; }
    }
}