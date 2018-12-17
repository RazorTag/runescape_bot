using RunescapeBot.BotPrograms;
using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms
{
    public class Conversation
    {
        public Conversation(TextBoxTool textBox)
        {
            TextBox = textBox;
            OtherPlayerChatRows = new bool[CHAT_ROW_COUNT];

            _scanChat = true;
            Thread scanChat = new Thread(ScanChat);
            scanChat.Start();
        }


        #region properties

        /// <summary>
        /// Number of rows visible in the public chat history.
        /// </summary>
        public const int CHAT_ROW_COUNT = 8;

        /// <summary>
        /// The text box container that hosts player conversations.
        /// </summary>
        private TextBoxTool TextBox { get; set; }

        /// <summary>
        /// Run parameters that describe the state of the bot program.
        /// </summary>
        private RunParams RunParams { get; set; }

        /// <summary>
        /// The latest image of the game screen.
        /// </summary>
        private Color[,] GameScreen { get; set; }

        /// <summary>
        /// Only scan the chat when set to true;
        /// </summary>
        private bool _scanChat { get; set; }

        /// <summary>
        /// True for the indices corresponding with rows populated with text from other players.
        /// </summary>
        private bool[] OtherPlayerChatRows { get; set; }
        private bool[] _previousChatRows { get; set; }

        #endregion

        /// <summary>
        /// Updates the local reference to the game screen.
        /// </summary>
        /// <param name="gameScreen">New reference to the latest game screen image.</param>
        public void SetScreen(Color[,] gameScreen)
        {
            GameScreen = gameScreen;
        }

        /// <summary>
        /// Stop scanning the chat.
        /// </summary>
        public void Stop()
        {
            _scanChat = false;
        }

        /// <summary>
        /// Periodically scans the chat to determine if the player needs to say something to another player.
        /// </summary>
        private void ScanChat()
        {
            while (_scanChat == true && !BotProgram.StopFlag)
            {
                DetermineOtherPlayerChatRows();
                if (MatchCurrentWithPreviousChat())
                {
                    //TODO alert the server
                }
            }
        }

        /// <summary>
        /// Determines which rows in the public chat history are populated by chat from other players.
        /// </summary>
        private void DetermineOtherPlayerChatRows()
        {
            _previousChatRows = OtherPlayerChatRows;

            for (int i = 0; i < CHAT_ROW_COUNT; i++)
            {
                OtherPlayerChatRows[i] = PlayerChatRow(i);
            }
        }

        /// <summary>
        /// Determines if a row contains chat from a player (including the player).
        /// </summary>
        /// <param name="row">The row to check for chat from a player.</param>
        /// <returns>True if the specified row contains chat from a player.</returns>
        private bool PlayerChatRow(int row)
        {
            RectangleBounds chatRow = TextBox.ChatRowLocation(row);
            double playerChatTextMatch = ImageProcessing.FractionalMatchPiece(GameScreen, TextBoxTool.PlayerChatText, chatRow.Left, chatRow.Right, chatRow.Top, chatRow.Bottom);
            bool containsPlayerText = playerChatTextMatch > 0.001;
            return containsPlayerText;
        }

        /// <summary>
        /// Determines if the first row of chat is another player's message.
        /// </summary>
        /// <returns>True if the first row of chat is another's player's text.</returns>
        private bool OtherPlayerHasTalked()
        {
            //TODO
            return false;
        }

        /// <summary>
        /// Determines if another player has spoken since the player last spoke.
        /// </summary>
        /// <returns>True if another player has spoken last.</returns>
        private bool MatchCurrentWithPreviousChat()
        {
            //Determine the most recent time that a player had talked previously.
            int firstPreviousChat = FirstTrue(_previousChatRows);
            int firstCurrentChat = FirstTrue(OtherPlayerChatRows);

            //If a chat message shows up more recently than the most recent prior message, then another player has talked.
            if (firstCurrentChat < firstPreviousChat)
            {
                return true;
            }

            //Determine if the current chat is a scrolled up version of the prior chat
            int scroll = firstCurrentChat - firstPreviousChat;
            for (int i = firstCurrentChat; i < CHAT_ROW_COUNT; i++)
            {
                if (OtherPlayerChatRows[i] != _previousChatRows[i - scroll])
                {
                    return false;
                }
            }

            return true;    //Current chat appears to be a scrolled up version of the prior chat.
        }

        /// <summary>
        /// Finds the index of the first true value in a boolean array.
        /// </summary>
        /// <param name="array">Arbitrary boolean array.</param>
        /// <returns>The first true index.</returns>
        private int FirstTrue(bool[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
