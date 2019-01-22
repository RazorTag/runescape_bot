using RunescapeBot.UITools;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading;
using RunescapeBot.Networking;

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
        /// Keyboard used for typing messages in chat
        /// </summary>
        private Keyboard Keyboard;

        /// <summary>
        /// Stores chat rows while they are being processed.
        /// </summary>
        internal ChatAssemblyLine ChatAssemblyLine;

        /// <summary>
        /// Networking utilities
        /// </summary>
        private readonly HttpClient httpClient = HttpInstance.HttpClient;

        /// <summary>
        /// The different "speakers" for a line of text for the most recent check.
        /// </summary>
        public ChatRow[] ChatRows
        {
            get { return _chatRows; }
            set
            {
                _chatRows = value;
                _newChatLines = null;
            }
        }
        private ChatRow[] _chatRows;

        /// <summary>
        /// The different "speakers" for a line of text for the penultimate check.
        /// </summary>
        private ChatRow[] PreviousChatRows;

        /// <summary>
        /// Number of new chat lines in the ChatRows/_previousChatRows diff
        /// </summary>
        internal int NewChatLines
        {
            get
            {
                if (_newChatLines == null)
                {
                    int line = 0;
                    while (line < CHAT_ROW_COUNT && !ChatRows[line].Equals(PreviousChatRows[0]))
                    {
                        line++;
                    }
                    _newChatLines = line;
                }
                return (int)_newChatLines;
            }
            set { _newChatLines = value; }
        }
        private int? _newChatLines;

        /// <summary>
        /// Identifier for the name tag of the player.
        /// </summary>
        internal string PlayerName
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

        /// <summary>
        /// Set to true to allow Conversation to use Keyboard to reply in chat.
        /// </summary>
        internal bool SafeToType
        {
            get { return _safeToType; }
            set
            {
                if (_safeToType = value)
                {
                    WriteResponses();
                }
            }
        }
        private bool _safeToType;

        /// <summary>
        /// Method to be called when we the chat server sends a response to a chat message.
        /// </summary>
        /// <param name="chatResponse"></param>
        public delegate void ChatServerCallback(string chatResponse);

        #endregion

        public Conversation(TextBoxTool textBox, GameScreen screen, Keyboard keyboard, bool conversate)
        {
            TextBox = textBox;
            Screen = screen;
            Keyboard = keyboard;

            SafeToType = false;
            
            if (_scanChat = conversate)
                StartConversation();
        }

        /// <summary>
        /// Begins scanning chat and responding to other players.
        /// </summary>
        public void StartConversation()
        {
            ChatAssemblyLine = new ChatAssemblyLine();
            ChatRows = new ChatRow[CHAT_ROW_COUNT];
            PreviousChatRows = new ChatRow[CHAT_ROW_COUNT];

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
        internal void ScanChat()
        {
            if (!ReadChatRows())
                return; //unable to read chat this time

            FindChatChanges();
            SendComments();
            WriteResponses();
        }

        /// <summary>
        /// Determines which rows in the public chat history are populated by chat from other players.
        /// </summary>
        internal bool ReadChatRows()
        {
            Color[,] textBoxImage = TextBox.TextBoxImage;
            if (textBoxImage == null)
                return false;

            PreviousChatRows = ChatRows;
            ChatRows = new ChatRow[CHAT_ROW_COUNT];
            Color[,] chatRowImage;

            for (int row = 0; row < CHAT_ROW_COUNT; row++)
            {
                chatRowImage = TextBoxTool.ChatRowImage(textBoxImage, row);
                ChatRows[row] = new ChatRow(chatRowImage, PlayerName);
            }

            return true;
        }

        /// <summary>
        /// Catalogs the new chat messages from other players.
        /// </summary>
        internal void FindChatChanges()
        {
            for (int i = 0; i < NewChatLines; i++)
            {
                if(ChatRows[i].Type == ChatRow.RowType.OtherPlayer)
                    ChatAssemblyLine.NewComment(ChatRows[i]);
            }
        }

        /// <summary>
        /// Uploads all of the waiting new comments from other players to the chat server.
        /// </summary>
        internal void SendComments()
        {
            ChatRow chatRow;

            while (!BotProgram.StopFlag && ChatAssemblyLine.NextComment(out chatRow))
            {
                Thread contactChatServer = new Thread(SendAndReceiveResponse);
                contactChatServer.Start(chatRow);
            }
        }

        /// <summary>
        /// Receives a response to a message from the chat server.
        /// </summary>
        public void SendAndReceiveResponse(object chatRow)
        {
            //FormUrlEncodedContent encodedRow = chatRow.Encode();

            //TODO send and receive a response from the chat server

            int ID = 0;
            string message = "";
            var response = new Response(ID, message);
            ChatAssemblyLine.ResponseReceived(response);
            WriteResponses();
        }

        /// <summary>
        /// Writes responses in the chat while it is safe to type.
        /// </summary>
        public void WriteResponses()
        {
            string response;

            while (!BotProgram.StopFlag &&
                SafeToType &&
                ChatAssemblyLine.NextResponse(out response) &&
                Screen.IsLoggedIn())
            {
                Keyboard.WriteLine(response);
            }
        }
    }
}
