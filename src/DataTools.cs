using System;
using System.Collections.Generic;
using System.Linq;
using TelegramHistory.Data;
using TLSharp.Core.MTProto;

namespace TelegramHistory
{
    public static class DataTools
    {
        public static List<MessageData> ParseMessageData(List<Message> tlMessages)
        {
            var messages = new List<MessageData>();

            foreach (var tlMessage in tlMessages.OfType<MessageConstructor>())
            {
                var message = new MessageData
                {
                    MessageId = tlMessage.id,
                    FromUserId = tlMessage.from_id,
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(tlMessage.date),
                    Text = tlMessage.message,
                    MessageType = MessageType.Text
                };

                var mediaDocument = tlMessage.media as MessageMediaDocumentConstructor;
                var document = mediaDocument?.document as DocumentConstructor;
                if (document != null)
                {
                    if (document.attributes.OfType<DocumentAttributeStickerConstructor>().Any())
                    {
                        message.MessageType = MessageType.Sticker;
                    }
                    else
                    {
                        message.MessageType = MessageType.Document;

                        var filename = document.attributes.OfType<DocumentAttributeFilenameConstructor>().FirstOrDefault();
                        message.Filename = filename?.file_name;
                    }
                }

                var mediaPhoto = tlMessage.media as MessageMediaPhotoConstructor;
                if (mediaPhoto != null)
                {
                    message.MessageType = MessageType.Photo;
                }

                var mediaVideo = tlMessage.media as MessageMediaVideoConstructor;
                if (mediaVideo != null)
                {
                    message.MessageType = MessageType.Video;
                }

                var mediaAudio = tlMessage.media as MessageMediaAudioConstructor;
                if (mediaAudio != null)
                {
                    message.MessageType = MessageType.Audio;
                }

                messages.Add(message);
            }

            return messages;
        }

        public static List<UserData> ParseUserData(List<User> tlUsers)
        {
            var contacts = tlUsers.OfType<UserContactConstructor>().Select(tlUser => new UserData
            {
                UserId = tlUser.id,
                FirstName = tlUser.first_name,
                LastName = tlUser.last_name
            });

            var self = tlUsers.OfType<UserSelfConstructor>().Select(tlUser => new UserData
            {
                UserId = tlUser.id,
                FirstName = tlUser.first_name,
                LastName = tlUser.last_name
            });

            return contacts.Union(self).ToList();
        }
    }
}