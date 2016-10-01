using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TelegramHistory.Data
{
    [DataContract]
    public class MessageCollection
    {
        [DataMember(Name = "messages")]
        public List<MessageData> Messages { get; set; }
    }
}