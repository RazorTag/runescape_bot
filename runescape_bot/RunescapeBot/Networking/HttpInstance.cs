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
        public static readonly string ChatServer = "http://www.evannorsworthy.com/OSRS-chat";

        /// <summary>
        /// URL to send a POST request to the API
        /// </summary>
        public static readonly string ChatServerApiPost = ChatServer + "/values";


        public static readonly string ChatServerApiGet = ChatServer + "/values/5";


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
            HttpClient.Timeout = new TimeSpan(0, 0, 30);
        }
    }
}
