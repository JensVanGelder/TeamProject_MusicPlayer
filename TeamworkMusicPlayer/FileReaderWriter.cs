using System;
using System.IO;

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

        public void ReadDataFromFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}