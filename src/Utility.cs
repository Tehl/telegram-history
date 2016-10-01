using System;
using System.Globalization;
using System.IO;

namespace TelegramHistory
{
    public static class Utility
    {
        public static string CharacterSafeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar.ToString(CultureInfo.InvariantCulture), "-");
            }
            while (fileName.Contains("--"))
            {
                fileName = fileName.Replace("--", "-");
            }
            return fileName;
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}