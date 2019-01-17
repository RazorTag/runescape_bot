using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot
{
    public class Networking
    {
        /// <summary>
        /// http client used to make server calls
        /// </summary>
        public static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// base URL of the chat server
        /// </summary>
        public static readonly string ChatServer = "http://www.evannorsworthy.com/OSRS-chat/";


        public Networking Instance
        {
            get
            {
                return _instance ?? (_instance = new Networking());
            }
        }
        private Networking _instance;


        private Networking()
        {
            HttpClient.BaseAddress = new Uri(ChatServer);
        }
    }
}
