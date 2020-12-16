using System.IO;
using System;
using WMPLib;

namespace TeamworkMusicPlayer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FileManager filemanager = new FileManager();
            Control control = new Control();
            filemanager.CreateFile(control.LogPath);
            control.MainMenu();
        }
    }
}