using System;
using System.IO;
using System.Collections.Generic;

namespace TeamworkMusicPlayer
{
    internal class FileReaderWriter
    {
        public void WriteDataTofile(string textToWriteTofile, string path)
        {
            using StreamWriter writer = new StreamWriter(path, true);
            DateTime date = new DateTime();
            date = DateTime.Now;
            writer.WriteLine($"{textToWriteTofile} - {date}");
        }
        public void WritePlaylistToFile(string textToWriteTofile, string path)
        {
            using StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine($"{textToWriteTofile}");
        }

        public void ReadDataFromFile(string path)
        {
            string text = File.ReadAllText(path);
            Console.WriteLine(text);
        }

    }
}