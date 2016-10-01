using System;
using TLSharp.Core;

namespace TelegramHistory
{
    public static partial class Telegram
    {
        public static bool BeginSession(string phoneNumber, TelegramClient client)
        {
            Console.WriteLine("Starting new user session for " + phoneNumber);
            string hash = null;
            if (!AsyncTask.RunAndWait(async () => hash = await client.SendCodeRequest(phoneNumber)))
            {
                return false;
            }

            Console.WriteLine("Input authentication code:");
            var code = Console.ReadLine();

            return AsyncTask.RunAndWait(async () => await client.MakeAuth(phoneNumber, hash, code));
        }
    }
}