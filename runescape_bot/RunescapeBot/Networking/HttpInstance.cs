using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.Networking
{
    public class HttpInstance
    {
        /// <summary>
        /// http client used to make server calls
        /// </summary>
        public static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// base URL of the chat server
        /// </summary>
        public static readonly string ChatServer = "http://www.evannorsworthy.com/OSRS-chat/";


        public static HttpInstance Instance
        {
            get
            {
                return _instance ?? (_instance = new HttpInstance());
            }
        }
        private static HttpInstance _instance;


        private HttpInstance()
        {
            HttpClient.BaseAddress = new Uri(ChatServer);
        }

        public async Task<string> JsonPost(string uri, FormUrlEncodedContent content)
        {
            HttpResponseMessage result = await HttpClient.PostAsync(uri, content);
            string responseString = await result.Content.ReadAsStringAsync();
            return responseString;
        }

        public async Task JsonPost(string uri, IUrlEncodable request)
        {
            var content = new FormUrlEncodedContent(request.GetUrlDictionary());
            await JsonPost(uri, request);
        }
    }
}
