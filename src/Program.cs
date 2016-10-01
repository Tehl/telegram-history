using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using TelegramHistory.Data;
using TLSharp.Core;
using TLSharp.Core.MTProto;

namespace TelegramHistory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            // get phone number
            string phoneNumber = ConfigurationManager.AppSettings["PhoneNumber"];
            if (String.IsNullOrEmpty(phoneNumber))
            {
                Console.WriteLine("Enter phone number in international format:");
                phoneNumber = Console.ReadLine();
            }

            // initialize client connection
            TelegramClient client;
            if (!Telegram.InitializeClient(phoneNumber, out client))
            {
                return;
            }

            // select a contact to export
            UserContactConstructor selectedContact;
            if (!Telegram.SelectContact(client, out selectedContact))
            {
                return;
            }

            // prep export directory
            var exportDirectory = Export.GetExportDirectory(selectedContact.phone);

            // start export
            StreamWriter jsonOutput = null;
            var knownUsers = new Dictionary<int, UserData>();
            try
            {
                int offset = 0;

                int receivedMessages;
                int currentYear = 0;
                int currentMonth = 0;
                bool first = true;

                // api only returns 100 messages at a time
                const int messagesPerPass = 100;

                do
                {
                    // retrieve the next batch of messages
                    var currentOffset = offset;
                    MessageHistory history = null;
                    if (!AsyncTask.RunAndWait(async () => history = await client.GetMessagesHistoryForContact(selectedContact.id, currentOffset, messagesPerPass)))
                    {
                        return;
                    }

                    // record recieved messages
                    receivedMessages = history.Messages.Count;
                    offset += receivedMessages;

                    // parse known users
                    var userData = DataTools.ParseUserData(history.Users);
                    foreach (var newUser in userData.Where(o => !knownUsers.ContainsKey(o.UserId)))
                    {
                        knownUsers.Add(newUser.UserId, newUser);
                    }

                    // parse messages
                    var messages = DataTools.ParseMessageData(history.Messages);
                    foreach (var message in messages)
                    {
                        // split output across multiple files for legibility
                        if (message.Timestamp.Year != currentYear || message.Timestamp.Month != currentMonth)
                        {
                            jsonOutput = Export.OpenJsonExport(exportDirectory, message.Timestamp.Year, message.Timestamp.Month, jsonOutput);
                            currentYear = message.Timestamp.Year;
                            currentMonth = message.Timestamp.Month;
                            first = true;
                        }

                        // write formatted message data
                        Export.WriteJsonMessage(message, jsonOutput, first);
                        first = false;
                    }

                    // display progress
                    Utility.ClearCurrentConsoleLine();
                    Console.Write($"Processed {offset} messages");

                    // sleep for 0.5s to avoid api spam
                    System.Threading.Thread.Sleep(500);
                } while (receivedMessages == messagesPerPass);

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception: " + ex.Message);
            }
            finally
            {
                Export.CloseJsonExport(jsonOutput);
            }

            // messages come back in newest->oldest order, so we need to reverse the output for human-readable exports
            Export.CreateChatLogs(exportDirectory, knownUsers);

            // export user data
            Export.WriteKnownUsers(exportDirectory, knownUsers);

            // finished!
            Console.WriteLine("Press Enter to quit");
            Console.ReadLine();
        }
    }
}