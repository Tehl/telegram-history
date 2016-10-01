using System;
using System.Linq;
using TLSharp.Core;
using TLSharp.Core.MTProto;

namespace TelegramHistory
{
    public static partial class Telegram
    {
        public static bool SelectContact(TelegramClient client, out UserContactConstructor selectedContact)
        {
            ContactsContacts contactData = null;
            if (!AsyncTask.RunAndWait(async () => { contactData = await client.GetContacts(); }))
            {
                selectedContact = null;
                return false;
            }

            var contacts = contactData.Users.OfType<UserContactConstructor>().ToList();
            if (!contacts.Any())
            {
                Console.WriteLine("No contacts to export");
                selectedContact = null;
                return false;
            }

            Console.WriteLine("Available contacts:");
            for (int i = 0; i < contacts.Count; i++)
            {
                var contact = contacts[i];

                var name = String.Join(" ", new[] {contact.first_name, contact.last_name}.Where(o => !String.IsNullOrEmpty(o)));
                if (!String.IsNullOrEmpty(contact.username))
                {
                    name += " (" + contact.username + ")";
                }

                name = name.Trim();
                var idx = i + 1;

                Console.WriteLine(idx + ": " + name);
            }

            Console.WriteLine();
            Console.WriteLine("Select contact by id:");
            int contactIdx = -1;
            while (true)
            {
                var input = Console.ReadLine();

                if (String.IsNullOrEmpty(input))
                {
                    contactIdx = -1;
                    break;
                }

                if (Int32.TryParse(input, out contactIdx) && contacts.Count >= (contactIdx + 1))
                {
                    break;
                }

                Console.WriteLine("Unknown contact, please try again or press Enter to quit");
            }

            if (contactIdx < 0)
            {
                selectedContact = null;
                return false;
            }

            selectedContact = contacts[contactIdx - 1];

            return true;
        }
    }
}