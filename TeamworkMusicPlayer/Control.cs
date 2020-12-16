using System;
using System.IO;
using System.Threading;
using WMPLib;

namespace TeamworkMusicPlayer
{
    internal class Control
    {
        public string Path { get; set; }
        public string Folder { get; set; }
        public string ChosenPlaylist { get; set; }
        public string[] Playlist { get; set; }
        public int Volume { get; set; } = 15;
        public string SongName { get; set; }
        public bool IsPaused { get; set; }
        public bool IsMuted { get; set; }
        public string LogPath = "D:/VDAB/Week3/TeamworkMusicPlayer/Log.txt";   //Hardcoded path for now
        private WindowsMediaPlayer player = new WindowsMediaPlayer();
        private FileReaderWriter readerWriter = new FileReaderWriter();
        private FileManager fileManager = new FileManager();
        private ConsoleKeyInfo cki;

        private string asciiTitle = @"
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
            player.settings.volume = Volume;
            IsPaused = false;
            player.settings.setMode("loop", true);
            Console.Write("Press a button to continue.");
            Console.ReadLine();
            Console.Clear();
            OptionSelectMenu();
        }

        public void OptionSelectMenu()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine(asciiTitle);
            Console.Write("Controls: " +
                "\n1.Play song" +
                "\n2.Play playlist" +
                "\n3.Create playlist" +
                "\n4.Show play history" +
                "\n5.Quit player");
            ConsoleKeyInfo cki;
            cki = Console.ReadKey(true);

            if (cki.Key == ConsoleKey.NumPad1)
            {
                GetSongPath();
                PlayMusic(Path);
            }
            else if (cki.Key == ConsoleKey.NumPad2)
            {
                Console.Clear();
                Console.WriteLine(asciiTitle);
                PrintAllPlaylists();
                ChoosePlaylist();
            }
            else if (cki.Key == ConsoleKey.NumPad3)
            {
                CreatePlaylist();
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
                OptionSelectMenu();
            }
        }

        public void GetSongPath()
        {
            Console.Write("\nEnter the path to your file:");
            string file = Console.ReadLine();
            if (File.Exists(file))
            {
                Path = file;
            }
            else
            {
                ErrorMessage("ERROR File not found", 1000);
                Console.Clear();
                OptionSelectMenu();
            }
            Console.Clear();
        }

        public void GetFolder()
        {
            Console.Write("\nEnter the path to your folder:");
            string folder = Console.ReadLine();
            if (Directory.Exists(folder))
            {
                Folder = folder;
            }
            else
            {
                ErrorMessage("ERROR File not found", 1000);
                Console.Clear();
                OptionSelectMenu();
            }
            Console.Clear();
        }

        public void PlayMusic(string file)
        {
            SongName = file.Substring(file.LastIndexOf("/") + 1);
            player.URL = file;
            readerWriter.WriteDataTofile(SongName, LogPath);

            while (true)    //Needs to be fixed, loop needs to check if song is still playing (infinite loop atm)
            {
                UserInput();
            }
        }

        public void PlayPlaylist(string path)
        {
            string[] playlist = File.ReadAllLines(path);
            player.currentPlaylist.clear();
            foreach (string file in playlist)
            {
                WMPLib.IWMPMedia song = player.newMedia(file);
                player.currentPlaylist.appendItem(song);
            }

            player.controls.play();
            Console.Clear();
            while (true)
            {
                UserInputPlayList();
            }
        }

        public void ChoosePlaylist()
        {
            Console.Write("\nChoose playlist:");
            ChosenPlaylist = Console.ReadLine();
            if (File.Exists($"D:/VDAB/Week3/TeamWorkMusicPlayer/Playlists/{ChosenPlaylist}.txt"))
            {
                readerWriter.WriteDataTofile($"Playlist:{ChosenPlaylist}", LogPath);
                PlayPlaylist($"D:/VDAB/Week3/TeamWorkMusicPlayer/Playlists/{ChosenPlaylist}.txt");
            }
            else
            {
                ErrorMessage("ERROR", 1500);
                ChoosePlaylist();
            }
        }

        public void CreatePlaylist()
        {
            Console.Clear();
            Console.WriteLine(asciiTitle);
            Console.WriteLine("Name your playlist");
            string playlistName = Console.ReadLine();
            if (!File.Exists($"D:/VDAB/Week3/TeamWorkMusicPlayer/Playlists/{playlistName}.txt"))
            {
                Console.Write("\nCreate playlist from folder? (y/n)");
                cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.Y)
                {
                    CreatePlaylistFromFolder(playlistName);
                    Console.Clear();
                    OptionSelectMenu();
                }
                else if (cki.Key == ConsoleKey.N)
                {
                    ErrorMessage("NOT IMPLEMENTED YET returning to main menu", 1500);
                }
                else
                {
                    ErrorMessage("ERROR returning to main menu", 1500);
                }
            }
            else
            {
                ErrorMessage("ERROR name already taken", 50);
                Console.WriteLine("Overwrite playlist with new one? (y/n)");
                cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.Y)
                {
                    fileManager.DeleteFile($"D:/VDAB/Week3/TeamWorkMusicPlayer/Playlists/{playlistName}.txt");
                    CreatePlaylistFromFolder(playlistName);
                }
                else if (cki.Key == ConsoleKey.N)
                {
                    Console.WriteLine("Keeping old playlist. Please enter a new name.");
                    Thread.Sleep(1500);
                    CreatePlaylist();
                }
                else
                {
                    ErrorMessage("ERROR returning to main menu", 1500);
                }
            }
        }

        public void CreatePlaylistFromFolder(string playlistname)
        {
            Console.Write("\nEnter path to folder:");
            string folder = Console.ReadLine();
            string[] playlist = Directory.GetFiles(folder, "*.mp3");

            WritePlaylistTofile(playlistname, playlist);
        }

        public void WritePlaylistTofile(string playlistname, string[] playlist)
        {
            fileManager.CreatePlaylistFile($"D:/VDAB/Week3/TeamWorkMusicPlayer/Playlists/{playlistname}.txt");
            foreach (string item in playlist)
            {
                string newitem = item.Replace("\\", "/");
                readerWriter.WritePlaylistToFile(newitem, $"D:/VDAB/Week3/TeamWorkMusicPlayer/Playlists/{playlistname}.txt");
            }
            Console.WriteLine($"\n\nPlaylist '{playlistname}' created." +
                $"\nReturning to main menu");
            Thread.Sleep(3000);
            Console.Clear();
            OptionSelectMenu();
        }

        public void PrintAllPlaylists()
        {
            Console.WriteLine("Current playlists:\n");
            foreach (string item in Directory.GetFiles("D:/VDAB/Week3/TeamWorkMusicPlayer/Playlists", "*.txt"))
            {
                Console.WriteLine(System.IO.Path.GetFileName(item));
            }
        }

        public void UserInput()
        {
            Console.WriteLine(asciiTitle);
            ConsoleKeyInfo cki;
            Console.WriteLine("Controls: \n" +
                "\n1.Pause/Play" +
                "\n2.Mute/Unmute" +
                "\n3.Change volume" +
                "\n4.Choose new song" +
                "\n5.Stop playing song" +
                "\n6.Return to main menu" +
                "\n7.Quit player" +
                "\n\n==================================================================" +
                $"\n\nVolume level [{VolumeBar()}] {Volume}%" +
                $"\n\nCurrently playing:");
            SongData();
            cki = Console.ReadKey(true);

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
                GetSongPath();
                PlayMusic(Path);
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D5 || cki.Key == ConsoleKey.NumPad5)
            {
                player.controls.stop();
                IsPaused = true;
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D5 || cki.Key == ConsoleKey.NumPad5)
            {
                player.controls.stop();
                IsPaused = true;
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D6 || cki.Key == ConsoleKey.NumPad6)
            {
                player.close();
                Console.Clear();
                OptionSelectMenu();
            }
            else if (cki.Key == ConsoleKey.D7 || cki.Key == ConsoleKey.NumPad7)
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

        public void UserInputPlayList()
        {
            Console.WriteLine(asciiTitle);
            ConsoleKeyInfo cki;
            Console.WriteLine("Controls: \n" +
                "\n1.Pause/Play" +
                "\n2.Mute/Unmute" +
                "\n3.Change volume" +
                "\n4.Next song" +
                "\n5.Previous song" +
                "\n6.Change playlist" +
                "\n7.Return to main menu" +
                "\n8.Quit player" +
                "\n\n==================================================================" +
                $"\n\nVolume level [{VolumeBar()}] {Volume}%" +
                $"\n\nCurrently playing from playlist: {ChosenPlaylist}");
            Console.WriteLine($"Song: {player.currentMedia.name}");
            cki = Console.ReadKey(true);

            if (cki.Key == ConsoleKey.D1 || cki.Key == ConsoleKey.NumPad1)
            {
                PausePlay();
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D4 || cki.Key == ConsoleKey.NumPad4)
            {
                player.controls.next();
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D5 || cki.Key == ConsoleKey.NumPad5)
            {
                player.controls.previous();
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
            else if (cki.Key == ConsoleKey.D6 || cki.Key == ConsoleKey.NumPad6)
            {
                Console.WriteLine();
                PrintAllPlaylists();
                ChoosePlaylist();
                Console.Clear();
            }
            else if (cki.Key == ConsoleKey.D7 || cki.Key == ConsoleKey.NumPad7)
            {
                player.close();
                Console.Clear();
                OptionSelectMenu();
            }
            else if (cki.Key == ConsoleKey.D8 || cki.Key == ConsoleKey.NumPad8)
            {
                System.Environment.Exit(1);
                Console.Clear();
            }
            else
            {
                ErrorMessage("ERROR Input not found", 1500);
                Console.Clear();
                UserInputPlayList();
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
            for (int i = 0; i < Volume / 5; i++)
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

        public void PrintLog()
        {
            FileReaderWriter readerWriter = new FileReaderWriter();
            Console.Clear();
            readerWriter.ReadDataFromFile(LogPath);
            Console.Write("\nReturn to main menu?(y/n)");

            ConsoleKeyInfo cki;
            cki = Console.ReadKey(true);

            if (cki.Key == ConsoleKey.Y)
            {
                Console.Clear();
                OptionSelectMenu();
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

        public void ErrorMessage(string errorInfo, int sleepTimer)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{errorInfo}");
            Thread.Sleep(sleepTimer);
            Console.ResetColor();
        }
    }
}