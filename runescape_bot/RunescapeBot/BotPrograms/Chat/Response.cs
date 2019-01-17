using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Chat
{
    internal class Response
    {
        /// <summary>
        /// Unique identifier to associate this response with the comment to which it is responding
        /// </summary>
        public int ID;

        /// <summary>
        /// Message to be typed into chat.
        /// </summary>
        public string Message;


        public Response(int id, string message)
        {
            ID = id;
            Message = message;
        }

        /// <summary>
        /// Determines if another Response object is equal to this one.
        /// </summary>
        /// <param name="other">another Response object to compare</param>
        /// <returns>true if the other Response contains the same data as this Response</returns>
        public bool Equals(Response other)
        {
            return
                ID == other.ID &&
                Message == other.Message;
        }
    }
}
