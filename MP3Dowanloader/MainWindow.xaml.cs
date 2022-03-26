using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MP3Dowanloader
{
    public partial class MainWindow : Window
    {
        FolderBrowserDialog folder;
        List<Song> songs;
        public MainWindow()
        {
            InitializeComponent();
            folder = new FolderBrowserDialog();
            songs = new List<Song>();
        }

        private void BT_Click(object sender, RoutedEventArgs e)
        {
            SiteToString siteToString = new SiteToString();
            string site = siteToString.GetHtmlPage("https://ru.hitmotop.com/search?q=" + TB.Text);
            SongsParser parser = new SongsParser();
            songs = parser.GetSongs(site);
            foreach (Song x in songs)
            {
                LB.Items.Add(x.Name);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            folder.ShowDialog();
        }

        string GenerateFileName(string songName)
        {
            string result = "";
            for (int i = 0; i < songName.Length; ++i)
            {
                if (songName[i] == '?' ||
                    songName[i] == '\"' ||
                    songName[i] == '|' ||
                    songName[i] == '\\' ||
                    songName[i] == '*' ||
                    songName[i] == '<' ||
                    songName[i] == '>' ||
                    songName[i] == '"' ||
                    songName[i] == '^' ||
                    songName[i] == ':' ||
                    songName[i] == '/' ||
                    songName[i] == '\n'
                    ) continue;
                result += songName[i];
            }
            if (result.Length == 0)
            {
                result = "song";
            }
            result += ".mp3";
            return result;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (LB.SelectedIndex < 0) return;
            if (folder.SelectedPath.Length == 0 || folder.SelectedPath == null)
            {
                if (folder.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                { return; }
            }

            string path = folder.SelectedPath + "\\" + GenerateFileName(songs[LB.SelectedIndex].Name);
            WebClient client = new WebClient();
            client.DownloadFileAsync(new Uri(songs[LB.SelectedIndex].Url), path);
        }

        async void DownloadAll()
        {
            WebClient client = new WebClient();
            if (folder.SelectedPath.Length == 0 || folder.SelectedPath == null)
            {
                if (folder.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                { return; }
            }
            PB.Value = 0;
            PB.Maximum = songs.Count;
            for (int i = 0; i < songs.Count; ++i)
            {
                string path = folder.SelectedPath + "\\" + GenerateFileName((songs[i].Name));
                LB1.Content = songs[i].Name + ".mp3";
                await Task.Run(() => client.DownloadFile(new Uri(songs[i].Url), path));
                await Task.Run(() => Dispatcher.Invoke(new Action(() => { PB.Value += 1; })));
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DownloadAll();
        }
    }
}
