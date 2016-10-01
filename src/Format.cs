using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TelegramHistory.Data;

namespace TelegramHistory
{
    public static class Format
    {
        public static string ForChatLog(MessageData message, Dictionary<int, UserData> users)
        {
            UserData user;
            string name = users.TryGetValue(message.FromUserId, out user)
                ? user.FirstName
                : message.FromUserId.ToString();

            string content;
            switch (message.MessageType)
            {
                case MessageType.Sticker:
                    content = "<Sticker>";
                    break;

                case MessageType.Document:
                    content = "<Document> " + message.Filename;
                    break;

                case MessageType.Photo:
                    content = "<Photo>";
                    break;
                case MessageType.Video:
                    content = "<Video>";
                    break;
                case MessageType.Audio:

                    content = "<Audio>";
                    break;

                default:
                    content = message.Text.Replace("\n", "\n\t");
                    break;
            }

            return String.Format("{0:g} [{1}]: {2}", message.Timestamp, name, content);
        }

        public static string ForJson(MessageData message)
        {
            return JsonConvert.SerializeObject(message);
        }
    }
}