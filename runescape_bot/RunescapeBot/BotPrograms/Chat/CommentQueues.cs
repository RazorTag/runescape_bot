using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RunescapeBot.BotPrograms.Chat.ChatRow;

namespace RunescapeBot.BotPrograms.Chat
{
    public class MessageQueues
    {
        /// <summary>
        /// Counts the number of chat rows that have been added.
        /// Used to ID chat rows.
        /// </summary>
        private int ChatRowCounter;

        /// <summary>
        /// Comments from other players which have yet to be sent to the chat server.
        /// </summary>
        private Queue<ChatRow> Comments;

        /// <summary>
        /// Responses to comments that are waiting to be typed.
        /// </summary>
        private Queue<string> Responses;


        public MessageQueues()
        {
            ChatRowCounter = 0;
            Comments = new Queue<ChatRow>();
            Responses = new Queue<string>();
        }

        /// <summary>
        /// Adds a new chat row to wait to be sent to the chat server.
        /// </summary>
        /// <param name="chatRow">comment from another player</param>
        internal void NewComment(ChatRow chatRow)
        {
            chatRow.ID = ++ChatRowCounter;
            Comments.Enqueue(chatRow);
        }

        /// <summary>
        /// Removes all ChatRows from the new comments queue and returns them as a List.
        /// These comments should then be sent to the chat server by the caller.
        /// </summary>
        internal List<ChatRowRequest> RetrieveComments()
        {
            var commentRequests = new List<ChatRowRequest>();
            
            while (Comments.Count > 0)
            {
                commentRequests.Add(Comments.Dequeue().GetRequest());
            }
            return commentRequests;
        }

        /// <summary>
        /// Converts a ChatRow waiting on a response into a Response waiting to be typed.
        /// Does nothing if the response's ID does not match the ID of a ChatRow waiting for a response.
        /// </summary>
        /// <param name="response">Response to type in chat in game</param>
        internal void ResponseReceived(List<string> responses)
        {
            foreach (string response in responses)
                Responses.Enqueue(response);
        }

        /// <summary>
        /// Gets all of the Response messages waiting to be typed.
        /// </summary>
        /// <returns>List of messages to respond with in chat</returns>
        internal List<string> GetResponses()
        {
            List<string> responses = Responses.ToList();
            Responses.Clear();
            return responses;
        }
    }
}
