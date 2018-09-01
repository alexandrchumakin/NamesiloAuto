using System;
using System.Net.Http;
using System.Net.Http.Headers;
using ServiceStack;

namespace NamesiloAuto
{
    internal static class JsonContent
    {
        public static StringContent Create(string json)
        {
            return new StringContent(json) { Headers = { ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") } };
        }

        public static StringContent Create(object obj)
        {
            var json = obj.ToJson();
            return Create(json);
        }
    }

    public class ApiManager
    {
        private readonly HttpClient _client;

        public ApiManager(string url)
        {
            _client = new HttpClient { BaseAddress = new Uri(url) };
            _client.DefaultRequestHeaders.Add("Keep-Alive", "true");
        }

        public bool Login(string path, string user, string pw)
        {
            string response =
                _client.PostAsync(path, JsonContent.Create(string.Format("trigger=1&username={0}&password={1}", user, pw)))
                    .Result.Content.ReadAsStringAsync().Result;
            return response.Contains("Welcome back");
            //Console.Write(response);
        }

        public bool SetBid(string path, string bid)
        {
            string response =
                _client.PostAsync(path, JsonContent.Create(string.Format("trigger_auction=1&bid={0}&terms=1", bid)))
                    .Result.Content.ReadAsStringAsync().Result;
            if (response.Contains("<div class=\"error_message\">"))
            {
                Console.WriteLine("### Error: Cannot set bid '{0}' for url '{1}':", bid, path);
                int ind = response.IndexOf("<div class=\"error_message\">", StringComparison.InvariantCulture);
                response = response.Substring(ind + 28).TrimStart();
                response = response.Substring(0, response.IndexOf("</div>", StringComparison.InvariantCulture));
                Console.Write(response);
                return false;
            }
            if (response.Contains("Your bid has been accepted"))
            {
                Console.WriteLine("### Success: Bid '{0}' is set successfully for path '{1}'.", bid, path);
                return true;
            }
            //other cases
            try
            {
                int index = response.IndexOf("<div id=\"innerContentContainerContent\">",
                    StringComparison.InvariantCulture);
                response = response.Substring(index + 34).TrimStart();
                response = response.Substring(0,
                    response.IndexOf("<div class=\"clear\"></div>", StringComparison.InvariantCulture));
            }
            catch {}
            Console.WriteLine("Cannot initialize status of bid, response text: " + response);
            return false;            
        }

    }
}
