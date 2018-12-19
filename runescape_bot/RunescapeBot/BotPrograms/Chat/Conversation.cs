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
        private GameScreen Screen;

        /// <summary>
        /// Only scan the chat when set to true.
        /// </summary>
        private bool _scanChat { get; set; }

        /// <summary>
        /// The different "speakers" for a line of text for the most recent check.
        /// </summary>
        private ChatRowType[] OtherPlayerChatRows { get; set; }

        /// <summary>
        /// The different "speakers" for a line of text for the penultimate check.
        /// </summary>
        private ChatRowType[] _previousChatRows { get; set; }

        /// <summary>
        /// The different "speakers" for a line of text.
        /// </summary>
        private enum ChatRowType
        {
            Unknown,
            Empty,
            Game,
            Player,
            OtherPlayer
        }

        #endregion

        public Conversation(TextBoxTool textBox, GameScreen screen, bool conversate)
        {
            TextBox = textBox;
            Screen = screen;
            OtherPlayerChatRows = new ChatRowType[CHAT_ROW_COUNT];

            if (_scanChat = conversate)
                StartConversation();
        }

        /// <summary>
        /// Begins scanning chat and responding to other players.
        /// </summary>
        public void StartConversation()
        {
            _scanChat = true;
            Thread scanChat = new Thread(ScanChat);
            scanChat.Start();
        }

        /// <summary>
        /// Stop scanning the chat.
        /// </summary>
        public void Stop()
        {
            _scanChat = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RunChatScanner()
        {
            while (_scanChat == true && !BotProgram.StopFlag)
            {
                if (Screen.LooksValid() && Screen.IsLoggedIn())
                    ScanChat();

                BotProgram.SafeWait(10000);
            }
        }

        /// <summary>
        /// Periodically scans the chat to determine if the player needs to say something to another player.
        /// </summary>
        private void ScanChat()
        {
            DetermineChatRowSpeakers();
            if (MatchCurrentWithPreviousChat())
            {
                //TODO alert the server
            }
        }

        /// <summary>
        /// Determines which rows in the public chat history are populated by chat from other players.
        /// </summary>
        private void DetermineChatRowSpeakers()
        {
            _previousChatRows = OtherPlayerChatRows;

            for (int i = 0; i < CHAT_ROW_COUNT; i++)
            {
                OtherPlayerChatRows[i] = ChatRowSpeaker(i);
            }
        }

        
        private ChatRowType ChatRowSpeaker(int row)
        {
            RectangleBounds chatRow = TextBox.ChatRowLocation(row);
            if (PlayerChatRow(chatRow))
            {
                return OtherPlayer(chatRow) ? ChatRowType.OtherPlayer : ChatRowType.Player;
            }

            return ChatRowType.Unknown;
        }

        /// <summary>
        /// Determines if a row contains chat from a player (including the player).
        /// </summary>
        /// <param name="row">The row to check for chat from a player.</param>
        /// <returns>True if the specified row contains chat from a player.</returns>
        private bool PlayerChatRow(RectangleBounds chatRow)
        {
            double playerChatTextMatch = ImageProcessing.FractionalMatchPiece(Screen, TextBoxTool.PlayerChatText, chatRow.Left, chatRow.Right, chatRow.Top, chatRow.Bottom);
            bool containsPlayerText = playerChatTextMatch > 0.001;
            return containsPlayerText;
        }

        /// <summary>
        /// Determines if the first row of chat is another player's message.
        /// </summary>
        /// <returns>True if the first row of chat is another's player's text.</returns>
        private bool OtherPlayer(RectangleBounds chatRow)
        {
            //TODO
            return true;
        }

        /// <summary>
        /// Determines if another player has spoken since the player last spoke.
        /// </summary>
        /// <returns>True if another player has spoken last.</returns>
        private bool MatchCurrentWithPreviousChat()
        {
            return false;
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
