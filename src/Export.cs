using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TelegramHistory.Data;

namespace TelegramHistory
{
    public static class Export
    {
        public static void CloseJsonExport(StreamWriter writer)
        {
            if (writer != null)
            {
                writer.Write("] }");
                writer.Close();
            }
        }

        public static void CreateChatLogs(DirectoryInfo exportDirectory, Dictionary<int, UserData> knownUsers)
        {
            foreach (var file in exportDirectory.GetFiles("*.json"))
            {
                var json = File.ReadAllText(file.FullName);
                var data = JsonConvert.DeserializeObject<MessageCollection>(json);

                var sb = new StringBuilder();
                foreach (var message in data.Messages.OrderBy(o => o.Timestamp))
                {
                    sb.AppendLine(Format.ForChatLog(message, knownUsers));
                }

                File.WriteAllText(file.FullName.Replace(".json", ".txt"), sb.ToString());
            }
        }

        public static DirectoryInfo GetExportDirectory(string contactPhoneNumber)
        {
            var exportDirectory = new DirectoryInfo(Utility.CharacterSafeFileName("export-" + contactPhoneNumber));
            if (!exportDirectory.Exists)
            {
                exportDirectory.Create();
            }

            foreach (var file in exportDirectory.EnumerateFiles())
            {
                file.Delete();
            }

            return exportDirectory;
        }

        public static StreamWriter OpenJsonExport(DirectoryInfo exportDirectory, int year, int month, StreamWriter currentFile)
        {
            CloseJsonExport(currentFile);

            var fs = new FileStream(
                exportDirectory.FullName + "\\" + year + "-" + month + ".json",
                FileMode.Create,
                FileAccess.Write
            );

            var writer = new StreamWriter(fs);
            writer.Write("{ \"messages\": [");

            return writer;
        }

        public static void WriteJsonMessage(MessageData message, StreamWriter writer, bool first)
        {
            if (!first)
            {
                writer.Write(", ");
            }

            writer.Write(Format.ForJson(message));
        }

        public static void WriteKnownUsers(DirectoryInfo exportDirectory, Dictionary<int, UserData> knownUsers)
        {
            var json = JsonConvert.SerializeObject(new UserCollection {Users = knownUsers.Values.ToList()});
            File.WriteAllText(exportDirectory.FullName + "\\users.json", json);
        }
    }
}