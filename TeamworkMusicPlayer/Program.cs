namespace TeamworkMusicPlayer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FileManager filemanager = new FileManager();
            Control player = new Control();

            filemanager.CreateFile("D:/VDAB/Week3/TeamworkMusicPlayer/Log.txt");
            player.MainMenu();
        }
    }
}