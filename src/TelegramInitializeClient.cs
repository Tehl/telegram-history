using System;
using System.Configuration;
using TLSharp.Core;

namespace TelegramHistory
{
    public static partial class Telegram
    {
        public static bool InitializeClient(string phoneNumber, out TelegramClient initializedClient)
        {
            var apiId = Convert.ToInt32(ConfigurationManager.AppSettings["TelegramApiId"]);
            var apiHash = ConfigurationManager.AppSettings["TelegramApiHash"];

            var sessionStore = new FileSessionStore();
            var sessionKey = phoneNumber.GetHashCode().ToString();

            var client = new TelegramClient(sessionStore, sessionKey, apiId, apiHash);

            if (!AsyncTask.RunAndWait(async () => await client.Connect()))
            {
                initializedClient = null;
                return false;
            }

            if (client.IsUserAuthorized())
            {
                Console.WriteLine("Restored user session for " + phoneNumber);
                initializedClient = client;
                return true;
            }

            if (!BeginSession(phoneNumber, client))
            {
                initializedClient = null;
                return false;
            }

            Console.WriteLine("Session started successfully");
            initializedClient = client;
            return true;
        }
    }
}