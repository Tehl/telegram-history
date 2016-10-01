using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TelegramHistory.Data
{
    [DataContract]
    public class UserCollection
    {
        [DataMember(Name = "users")]
        public List<UserData> Users { get; set; }
    }
}