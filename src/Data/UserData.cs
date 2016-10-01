using System.Runtime.Serialization;

namespace TelegramHistory.Data
{
    [DataContract]
    public class UserData
    {
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "userId")]
        public int UserId { get; set; }
    }
}