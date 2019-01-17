using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Chat
{
    public class ChatAssemblyLine
    {
        /// <summary>
        /// Counts the number of chat rows that have been added.
        /// Used to ID chat rows.
        /// </summary>
        private int ChatRowCounter;

        /// <summary>
        /// Comments from other players which have yet to be sent to the chat server.
        /// </summary>
        private Queue<ChatRow> NewComments;

        /// <summary>
        /// Comments that have been sent to the chat server and are waiting on a response.
        /// </summary>
        private Dictionary<int, ChatRow> ServerComments;

        /// <summary>
        /// Responses to comments that are waiting to be typed.
        /// </summary>
        private Queue<Response> Responses;


        public ChatAssemblyLine()
        {
            ChatRowCounter = 0;
            NewComments = new Queue<ChatRow>();
            ServerComments = new Dictionary<int, ChatRow>();
            Responses = new Queue<Response>();
        }

        /// <summary>
        /// Adds a new chat row to wait to be sent to the chat server.
        /// </summary>
        /// <param name="chatRow">comment from another player</param>
        internal void NewComment(ChatRow chatRow)
        {
            chatRow.ID = ++ChatRowCounter;
            NewComments.Enqueue(chatRow);
        }

        /// <summary>
        /// Gets the next comment to be sent to the chat server.
        /// Assumes that the caller sends this ChatRow to the chat server.
        /// </summary>
        /// <param name="chatRow">returns the next chat row to be sent to the chat server</param>
        /// <returns>true if there is another comment to get</returns>
        internal bool NextComment(out ChatRow chatRow)
        {
            if (NewComments.Count == 0)
            {
                chatRow = null;
                return false;
            }

            chatRow = NewComments.Dequeue();
            ServerComments.Add(chatRow.ID, chatRow);
            return true;
        }

        /// <summary>
        /// Converts a ChatRow waiting on a response into a Response waiting to be typed.
        /// Does nothing if the response's ID does not match the ID of a ChatRow waiting for a response.
        /// </summary>
        /// <param name="response">Response to type in chat in game</param>
        internal void ResponseReceived(Response response)
        {
            if (ServerComments.ContainsKey(response.ID))
            {
                ServerComments.Remove(response.ID);
                Responses.Enqueue(response);
            }
        }

        /// <summary>
        /// Gets the next message to type into chat.
        /// </summary>
        /// <param name="responseMessage">returns the next message to type into chat</param>
        /// <returns>true if there is another response to get</returns>
        internal bool NextResponse(out string responseMessage)
        {
            if (Responses.Count == 0)
            {
                responseMessage = "";
                return false;
            }

            Response response = Responses.Dequeue();
            responseMessage = response.Message;
            return true;
        }
    }
}
