using System.IO;

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