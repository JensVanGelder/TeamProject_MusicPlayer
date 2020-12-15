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
        public string SongName { get; set; }
        public bool IsPaused { get; set; }
        public bool IsMuted { get; set; }
        private WindowsMediaPlayer player = new WindowsMediaPlayer();
        string asciiTitle = @"
.__   __.      ___      .______     _______.___________. _______ .______      .______      
|  \ |  |     /   \     |   _  \   /       |           ||   ____||   _  \     |   _  \     
|   \|  |    /  ^  \    |  |_)  | |   (----`---|  |----`|  |__   |  |_)  |    |  |_)  |    
|  . `  |   /  /_\  \   |   ___/   \   \       |  |     |   __|  |      /     |      /     
|  |\   |  /  _____  \  |  |   .----)   |      |  |     |  |____ |  |\  \----.|  |\  \----.
|__| \__| /__/     \__\ | _|   |_______/       |__|     |_______|| _| `._____|| _| `._____|  tm
                                                                                           
";

        public void MainMenu()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine(asciiTitle);
            Console.ReadLine();
            Console.Clear();
            SongSelectMenu();
        }
        public void SongSelectMenu()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine(asciiTitle);
            Greeting();
            GetPath();
            PlayMusic(Path);
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
                ErrorMessage("ERROR File not found", 1000);
                Console.Clear();
                GetPath();
            }
            Console.Clear();
        }

        public void PlayMusic(string file)
        {
            FileReaderWriter readerWriter = new FileReaderWriter();
            string logFile = "D:/VDAB_.net_C-/VDAB/Week3/TeamworkMusicPlayer/Log.txt";
            SongName = file.Substring(file.LastIndexOf("/") + 1);
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
                $"\nVolume level [{VolumeBar()}] {Volume}%" +
                $"\n\nCurrently playing:");
            SongData();
            cki = Console.ReadKey();

            if (cki.Key == ConsoleKey.D1 || cki.Key == ConsoleKey.NumPad1)
            {
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
                SongSelectMenu();
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
                ErrorMessage("ERROR Input not found", 1500);
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

        public void SetVolume()
        {
            Console.WriteLine($"\nThis is the current volume {Volume}" +
                        "\nSelect volume between 1/100");
            int volume = Convert.ToInt32(Console.ReadLine());
            if (volume > 100 || volume < 0)
            {
                ErrorMessage("ERROR volume not valid", 1500);
            }
            else
            {
                Volume = volume;
                player.settings.volume = Volume;
                //Console.WriteLine($"Titel: {media.getItemInfo("Title")}");

            }
        }

        public string VolumeBar()
        {
            string volumeBar = "";
            for (int i = 0; i < Volume/5; i++)
            {
                volumeBar += "#";
            }
            if (Volume <=4 && Volume >=1)
            {
                volumeBar = "#";
            }
            while (volumeBar.Length != 20)
            {
                volumeBar += " ";
            }
            return volumeBar;
        }
        
        public void SongData()
        {
            var song = TagLib.File.Create(Path);
            string title = song.Tag.Title;
            title = SongDataChecker(title);
            string artist = song.Tag.FirstAlbumArtist;
            artist = SongDataChecker(artist);
            string album = song.Tag.Album;
            album = SongDataChecker(album);
            string genre = song.Tag.FirstGenre;
            genre = SongDataChecker(genre);
            string duration = song.Properties.Duration.ToString(@"mm\:ss");


            Console.WriteLine($"" +
                $"\nSong:           {title}" +
                $"\nArtist:         {artist}" +
                $"\nAlbum:          {album}" +
                $"\nGenre:          {genre}" +
                $"\nDuration:       {duration}");
        }
        public string SongDataChecker(string tag)
        {
            if (String.IsNullOrEmpty(tag))
            {
                 tag = "Unknown";
            }
            return tag;
        }
        public void ErrorMessage(string errorInfo, int sleepTimer)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{errorInfo}");
            Thread.Sleep(sleepTimer);
            Console.ResetColor();
        }
    }
}