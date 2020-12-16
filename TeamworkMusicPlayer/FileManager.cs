using System.IO;
using System;
using System.Threading;

namespace TeamworkMusicPlayer
{
    internal class FileManager
    {
        public void CreateFile(string file)
        {
            if (!File.Exists(file))
            {
                FileStream fileStream = File.Create(file);
                fileStream.Close();
            }
        }
        public void CreatePlaylistFile(string file)
        {
            if (!File.Exists(file))
            {
                FileStream fileStream = File.Create(file);
                fileStream.Close();
            }
            else
            {
                Console.WriteLine("ERROR");
                Thread.Sleep(1500);
            }
        }

        public void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        public void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}