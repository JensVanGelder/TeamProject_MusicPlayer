using System;
using System.IO;
using System.Threading;
using WMPLib;
using System.Linq;

namespace TeamworkMusicPlayer
{
    internal class Control
    {
        public string Path { get; set; }
        public int Volume { get; set; }
        public string SongName { get; set; }
        private WindowsMediaPlayer player = new WindowsMediaPlayer();

        public void MainMenu()
        {
            Control player = new Control();
            player.Greeting();
            player.GetPath();
            player.PlayMusic(player.Path);
            Console.ReadLine();
        }

        public void Greeting()
        {
            Console.WriteLine("Welcome to your music player!");
        }

        public void GetPath()
        {
            Console.WriteLine("Enter the path to your file:");
            string file = Console.ReadLine();
            if (File.Exists(file))
            {
                Path = file;
            }
            else
            {
                Console.WriteLine("Error");
                Thread.Sleep(1000);
                Console.Clear();
                GetPath();
            }
            Console.Clear();
        }

        public void PlayMusic(string file)
        {
            FileReaderWriter readerWriter = new FileReaderWriter();
            string logFile = "D:/VDAB_.net_C-/VDAB/Week3/TeamworkMusicPlayer/Log.txt";
            SongName = file.Substring(file.LastIndexOf("/")+1);
            player.settings.volume = 5;
            player.URL = file;
            readerWriter.WriteDataTofile(SongName, logFile);

            while (true)    //Needs to be fixed, loop needs to check if song is still playing (infinite loop atm)
            {
                UserInput();
            }
        }

        public void UserInput()
        {
            Console.WriteLine($"Currently playing : {SongName}\n");
            Console.WriteLine("Controls: " +
                "\n'pause', 'resume', 'mute', 'unmute', 'volume', 'newsong', 'stop', 'exit'");
            string controlInput = Console.ReadLine();
            switch (controlInput)
            {
                case "pause":
                    player.controls.pause();
                    break;

                case "resume":
                    player.controls.play();
                    break;

                case "mute":
                    player.settings.mute = true;
                    break;

                case "unmute":
                    player.settings.mute = false;
                    break;

                case "volume":
                    SetVolume();
                    break;

                case "newsong":
                    player.close();
                    MainMenu();
                    break;

                case "stop":
                    player.controls.stop();
                    break;

                case "exit":
                    System.Environment.Exit(1);
                    break;

                default:
                    Console.WriteLine("Error wrong input");
                    Thread.Sleep(1000);
                    break;
            }
            Console.Clear();
        }

        public void SetVolume()
        {
            Console.WriteLine($"This is the current volume {Volume}" +
                        "\nSelect volume between 1/100");
            Volume = Convert.ToInt32(Console.ReadLine());
            if (Volume > 100 || Volume < 0)
            {
                Console.WriteLine("Error");
                Thread.Sleep(1000);
            }
            else
            {
                player.settings.volume = Volume;
            }
        }
    }
}