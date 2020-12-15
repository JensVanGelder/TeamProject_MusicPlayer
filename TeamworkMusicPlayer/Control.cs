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
        public int Volume { get; set; } = 15;
        public string SongName { get; set; }
        public bool IsPaused { get; set; }
        public bool IsMuted { get; set; }
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
            player.settings.volume = Volume;
            IsPaused = false;
            player.URL = file;
            readerWriter.WriteDataTofile(SongName, logFile);

            while (true)    //Needs to be fixed, loop needs to check if song is still playing (infinite loop atm)
            {
                UserInput();
            }
        }

        public void UserInput()
        {
            ConsoleKeyInfo cki;
            Console.WriteLine("Controls: " +
                "\n1.Pause/Play" +
                "\n2.Mute/Unmute" +
                "\n3.Change volume" +
                "\n4.Choose new song" +
                "\n5.Stop playing song" +
                "\n6.Quit player" +
                "\n\n==================================================================" +
                $"\n\nCurrently playing: {SongName}" +
                $"\nVolume level {Volume}%");
            cki = Console.ReadKey();

            if (cki.Key == ConsoleKey.D1|| cki.Key == ConsoleKey.NumPad1)
            {
                //shfvhjdgfjkhgsd fvjkgjsdkv
                PausePlay();
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D2 || cki.Key == ConsoleKey.NumPad2)
            {
                MuteUnmute();
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D3 || cki.Key == ConsoleKey.NumPad3)
            {
                SetVolume();
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D4 || cki.Key == ConsoleKey.NumPad4)
            {
                player.close();
                MainMenu();
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D5 || cki.Key == ConsoleKey.NumPad5)
            {
                player.controls.stop();
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D6 || cki.Key == ConsoleKey.NumPad6)
            {
                System.Environment.Exit(1);
                Console.Clear();
            }
            else
            {
                Console.WriteLine("ERROR");
                Thread.Sleep(1000);
                Console.Clear();
                UserInput();
            }
            Console.Clear();
        }

        public void PausePlay()
        {
            if (IsPaused == true)
            {
                player.controls.play();
                IsPaused = false;
            }
            else
            {
                player.controls.pause();
                IsPaused = true;
            }
        }

        public void MuteUnmute()
        {
            if (IsMuted== true)
            {
                player.settings.mute = false;
                IsMuted = false;
            }
            else
            {
                player.settings.mute = true;
                IsMuted = true;
            }
        }

        public void SetVolume()
        {
            Console.WriteLine($"\nThis is the current volume {Volume}" +
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