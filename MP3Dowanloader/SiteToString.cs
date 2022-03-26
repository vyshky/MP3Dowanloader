using System.IO;
using System.Net;


namespace MP3Dowanloader
{
    public class SiteToString
    {
        public string GetHtmlPage(string url)
        {
            WebRequest request = WebRequest.Create(url);

            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();

            StreamReader sr = new StreamReader(stream);

            return sr.ReadToEnd();
        }
    }
}
