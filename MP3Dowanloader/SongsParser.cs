using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Dowanloader
{
    public class SongsParser
    {
        List<Song> songs;
        public List<Song> GetSongs(string site)
        {
            songs = new List<Song>();
            string name = "";
            string url = "";
            int index = site.IndexOf("track__title");
            while (index >= 0)
            {
                site = site.Remove(0, index + 15);
                index = site.IndexOf("</div");
                if (index >= 0)
                {
                    name = site.Remove(index);
                    index = site.IndexOf("data-nopjax");
                    site = site.Remove(0, index + 11);
                    index = site.IndexOf("href");
                    site = site.Remove(0, index + 6);
                    url = site.Remove(site.IndexOf("\""));
                    songs.Add(new Song() { Name = name.Trim(), Url = url });
                    index = site.IndexOf("track__title");
                }
            }
            return songs;
        }
    }
}
