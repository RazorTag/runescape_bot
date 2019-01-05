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

namespace RunescapeBot.BotPrograms.Chat
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
        private TextBoxTool TextBox;

        /// <summary>
        /// Run parameters that describe the state of the bot program.
        /// </summary>
        private RunParams RunParams;

        /// <summary>
        /// The latest image of the game screen.
        /// </summary>
        private GameScreen Screen;

        /// <summary>
        /// Translates images of chat rows into their string values.
        /// </summary>
        private LetterReader ChatReader;

        /// <summary>
        /// Only scan the chat when set to true.
        /// </summary>
        private bool _scanChat;

        /// <summary>
        /// The different "speakers" for a line of text for the most recent check.
        /// </summary>
        private ChatRow[] OtherPlayerChatRows;

        /// <summary>
        /// The different "speakers" for a line of text for the penultimate check.
        /// </summary>
        private ChatRow[] _previousChatRows;

        /// <summary>
        /// Identifier for the name tag of the player.
        /// </summary>
        private string PlayerName
        {
            get
            {
                if (string.IsNullOrEmpty(_playerName))
                {
                    var chatRow = new ChatRow(TextBox.InputImage, "");
                    _playerName = chatRow.SpeakerName;
                }
                return _playerName;
            }
        }
        private string _playerName;

        #endregion

        public Conversation(TextBoxTool textBox, GameScreen screen, bool conversate)
        {
            TextBox = textBox;
            Screen = screen;
            
            if (_scanChat = conversate)
                StartConversation();
        }

        /// <summary>
        /// Begins scanning chat and responding to other players.
        /// </summary>
        public void StartConversation()
        {
            _scanChat = true;
            OtherPlayerChatRows = new ChatRow[CHAT_ROW_COUNT];
            _playerName = "";
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
        /// Keep scanning the chat until told to stop.
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
            Color[,] chatBody = TextBox.TextBoxImage;

            ReadChatRows(chatBody);
            if (MatchCurrentWithPreviousChat())
            {
                //TODO alert the server
            }
        }

        /// <summary>
        /// Determines which rows in the public chat history are populated by chat from other players.
        /// </summary>
        private void ReadChatRows(Color[,] chatBody)
        {
            _previousChatRows = OtherPlayerChatRows;
            Color[,] chatRowImage;

            for (int row = 0; row < CHAT_ROW_COUNT; row++)
            {
                chatRowImage = TextBox.ChatRowImage(row);
                OtherPlayerChatRows[row] = new ChatRow(chatRowImage, PlayerName);
            }
        }

        /// <summary>
        /// Determines if another player has spoken since the player last spoke.
        /// </summary>
        /// <returns>True if another player has spoken last.</returns>
        private bool MatchCurrentWithPreviousChat()
        {
            return false;
        }
    }
}
