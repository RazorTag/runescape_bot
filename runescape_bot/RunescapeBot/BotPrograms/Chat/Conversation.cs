using System.Drawing;
using System.Threading;

namespace RunescapeBot.BotPrograms.Chat
{
    public class Conversation
    {
        #region properties

        /// <summary>
        /// Number of rows visible in the public chat history.
        /// </summary>
        public const int CHAT_ROW_COUNT = TextBoxTool.CHAT_ROW_COUNT;

        /// <summary>
        /// Only scan the chat when set to true.
        /// </summary>
        private bool _scanChat;

        /// <summary>
        /// The text box container that hosts player conversations.
        /// </summary>
        private TextBoxTool TextBox;

        /// <summary>
        /// The latest image of the game screen.
        /// </summary>
        private GameScreen Screen;

        /// <summary>
        /// The different "speakers" for a line of text for the most recent check.
        /// </summary>
        public ChatRow[] ChatRows;

        /// <summary>
        /// The different "speakers" for a line of text for the penultimate check.
        /// </summary>
        public ChatRow[] _previousChatRows;

        /// <summary>
        /// Identifier for the name tag of the player.
        /// </summary>
        public string PlayerName
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
            ChatRows = new ChatRow[CHAT_ROW_COUNT];
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
            ReadChatRows();

            if (MatchCurrentWithPreviousChat())
            {
                //TODO alert the server
            }
        }

        /// <summary>
        /// Determines which rows in the public chat history are populated by chat from other players.
        /// </summary>
        public void ReadChatRows()
        {
            _previousChatRows = ChatRows;
            ChatRows = new ChatRow[CHAT_ROW_COUNT];
            Color[,] chatRowImage;

            for (int row = 0; row < CHAT_ROW_COUNT; row++)
            {
                chatRowImage = TextBoxTool.ChatRowImage(TextBox.TextBoxImage, row);
                ChatRows[row] = new ChatRow(chatRowImage, PlayerName);
            }
        }

        /// <summary>
        /// Determines if another player has spoken since the player last spoke.
        /// </summary>
        /// <returns>True if another player has spoken last.</returns>
        public bool MatchCurrentWithPreviousChat()
        {
            return false;
        }
    }
}
