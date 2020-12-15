using System;
using System.IO;
using System.Threading;
using WMPLib;

namespace TeamworkMusicPlayer
{
    internal class Control
    {
        public string Path { get; set; }
        public int Volume { get; set; } = 15;
        public bool IsMuted { get; set; }
        public bool IsPlaying { get; set; }
        public string SongName { get; set; }
        private WindowsMediaPlayer player = new WindowsMediaPlayer();

        public string logPath = "D:/VDAB_.net_C-/VDAB/Week3/TeamworkMusicPlayer/Log.txt";

        public void MainMenu()
        {
            Control player = new Control();
            player.Greeting();
            string mainInput = Console.ReadLine();
            switch (mainInput)
            {
                case "1":
                    player.GetPath();
                    player.PlayMusic(player.Path);
                    Console.ReadLine();
                    break;

                case "2":
                    break;

                case "3":
                    break;

                case "4":
                    break;

                case "5":
                    System.Environment.Exit(1);
                    break;

                default:
                    ErrorMessage("ERROR action not found", 1000);
                    Console.Clear();
                    MainMenu();
                    break;
            }
        }

        public void MainMenuNoEnter()
        {
            Greeting();
            ConsoleKeyInfo cki;
            cki = Console.ReadKey();

            if (cki.Key == ConsoleKey.NumPad1)
            {
                GetPath();
                PlayMusic(Path);
                Console.ReadLine();
            }
            else if (cki.Key == ConsoleKey.NumPad4)
            {
                PrintLog();
            }
            else if (cki.Key == ConsoleKey.NumPad5)
            {
                System.Environment.Exit(1);
            }
            else
            {
                ErrorMessage("ERROR Control not found", 1000);
                Console.Clear();
                MainMenuNoEnter();
            }
        }

        public void PlayNewSong()
        {
            GetPath();
            PlayMusic(Path);
            Console.ReadLine();
        }

        public void Greeting()
        {
            Console.WriteLine("Welcome to your music player!" +
                "\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" +
                "\n1.Play song" +
                "\n2.Create setlist (NOT IMPLEMENTED)" +
                "\n3.Play setlist (NOT IMPLEMENTED)" +
                "\n4.Show log file" +
                "\n5.Close music player\n");
            Console.Write("Choose what you want to do:");
        }

        public void GetPath()
        {
            Console.WriteLine("\nEnter the path to your file:");
            string file = Console.ReadLine();
            if (File.Exists(file))
            {
                Path = file;
            }
            else
            {
                ErrorMessage("ERROR No file found, returning to Main Menu", 2000);
                Console.Clear();
                MainMenuNoEnter();
            }
            Console.Clear();
        }

        public void PlayMusic(string file)
        {
            FileReaderWriter readerWriter = new FileReaderWriter();
            SongName = file.Substring(file.LastIndexOf("/") + 1);
            player.settings.volume = Volume;
            player.URL = file;
            IsPlaying = true;
            IsMuted = false;
            readerWriter.WriteDataTofile(SongName, logPath);

            while (true) //Needs to be fixed, loop needs to check if song is still playing (infinite loop atm)
            {
                UserInputNoEnter();
            }
        }

        public void UserInput()
        {
            Console.WriteLine("Controls: " +
                "\n1. Pause/Play" +
                "\n2. Mute/Unmute" +
                "\n3. Change volume" +
                "\n4. Choose new song" +
                "\n5. Stop current song" +
                "\n6. Return to Main Menu" +
                "\n7. Exit player" +
                "\n\n=================================" +
                $"\nVolume is at {Volume}%" +
                $"\nCurrently playing : {SongName}\n");
            Console.Write("Enter controls:");
            string controlInput = Console.ReadLine();
            switch (controlInput)
            {
                case "1":
                    PauseAndPlay();
                    break;

                case "2":
                    MuteAndUnmute();
                    break;

                case "3":
                    SetVolume();
                    break;

                case "4":
                    player.close();
                    PlayNewSong();
                    break;

                case "5":
                    player.controls.stop();
                    break;

                case "6":
                    player.close();
                    Console.Clear();
                    MainMenu();
                    break;

                case "7":
                    System.Environment.Exit(1);
                    break;

                default:
                    ErrorMessage("ERROR Control not found", 1000);
                    break;
            }

            Console.Clear();
        }

        public void UserInputNoEnter()
        {
            Console.WriteLine("Controls: " +
                "\n1. Pause/Play" +
                "\n2. Mute/Unmute" +
                "\n3. Change volume" +
                "\n4. Choose new song" +
                "\n5. Stop current song" +
                "\n6. Return to Main Menu" +
                "\n7. Exit player" +
                "\n\n=================================\n" +
                "\nCurrently Playing:" +
                $"\n\nVolume [{DrawVolume()}]{Volume}%");
            CurrentlyPlayingInfo(Path);
            Console.Write("\nEnter controls:");
            ConsoleKeyInfo cki;

            cki = Console.ReadKey();

            if (cki.Key == ConsoleKey.NumPad1)
            {
                PauseAndPlay();
                Console.Clear();
                //do some thing
            }
            else if (cki.Key == ConsoleKey.NumPad2)
            {
                MuteAndUnmute();
                Console.Clear();
                //do some thing
            }
            else if (cki.Key == ConsoleKey.NumPad3)
            {
                SetVolume();
                Console.Clear();
                //do some thing
            }
            else if (cki.Key == ConsoleKey.NumPad4)
            {
                player.close();
                PlayNewSong();
                Console.Clear();
                //do some thing
            }
            else if (cki.Key == ConsoleKey.NumPad5)
            {
                player.controls.stop();
                IsPlaying = false;
                Console.Clear();
                //do some thing
            }
            else if (cki.Key == ConsoleKey.NumPad6)
            {
                player.close();
                Console.Clear();
                MainMenuNoEnter();
                //do some thing
            }
            else if (cki.Key == ConsoleKey.NumPad7)
            {
                System.Environment.Exit(1);
                //do some thing
            }
            else
            {
                ErrorMessage("ERROR Wrong input", 1000);
            }
            Console.Clear();
        }

        public void SetVolume()
        {
            Console.WriteLine($"\nSelect volume between 1 and 100");
            int volume = Convert.ToInt32(Console.ReadLine());
            if (volume > 100 || volume < 0)
            {
                ErrorMessage("ERROR volume not between 1 and 100", 1000);
            }
            else
            {
                Volume = volume;
                player.settings.volume = volume;
            }
        }

        public string DrawVolume()
        {
            string volumeBar = "";
            int volumeBarLength = volumeBar.Length;
            for (int i = 0; i < (Volume / 5); i++)
            {
                volumeBar += "#";
            }
            if (Volume <= 4 && Volume >= 1)
            {
                volumeBar = "#";
            }
            while (volumeBar.Length != 20)
            {
                volumeBar += " ";
            }

            return volumeBar;
        }

        public void PauseAndPlay()
        {
            if (IsPlaying == true)
            {
                player.controls.pause();
                IsPlaying = false;
            }
            else
            {
                player.controls.play();
                IsPlaying = true;
            }
        }

        public void MuteAndUnmute()
        {
            if (IsMuted == true)
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

        public void CurrentlyPlayingInfo(string path)
        {
            var song = TagLib.File.Create(@path);
            string title = song.Tag.Title;
            string artist = song.Tag.FirstAlbumArtist;
            string album = song.Tag.Album;
            string genre = song.Tag.FirstGenre;
            string duration = song.Properties.Duration.ToString(@"mm\:ss");
            Console.WriteLine($"" +
                $"\nSong:       {title}" +
                $"\nArtist:     {artist}" +
                $"\nAlbum:      {album}" +
                $"\nGenre:      {genre}" +
                $"\nDuration:   {duration}");
        }

        public void PrintLog()
        {
            FileReaderWriter readerWriter = new FileReaderWriter();
            Console.Clear();
            readerWriter.ReadDataFromFile(logPath);
            Console.Write("\nReturn to main menu?(y/n)");

            ConsoleKeyInfo cki;
            cki = Console.ReadKey();

            if (cki.Key == ConsoleKey.Y)
            {
                Console.Clear();
                MainMenuNoEnter();
            }
            else if (cki.Key == ConsoleKey.N)
            {
                System.Environment.Exit(1);
            }
            else
            {
                ErrorMessage("ERROR Wrong input", 1000);
                Console.Clear();
                PrintLog();
            }
        }

        public void ErrorMessage(string errormessage, int sleepTimer)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{errormessage}");
            Console.ResetColor();
            Thread.Sleep(sleepTimer);
        }
    }
}