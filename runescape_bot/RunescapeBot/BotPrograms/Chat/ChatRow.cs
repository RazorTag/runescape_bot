using RunescapeBot.ImageTools;
using RunescapeBot.ImageTools.Filters.FilterFactories;
using RunescapeBot.Networking;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RunescapeBot.BotPrograms.Chat
{
    public class ChatRow : IUrlEncodable
    {
        #region properties

        /// <summary>
        /// The different "speakers" for a line of text.
        /// </summary>
        public enum RowType
        {
            Undetermined,
            Unknown,
            Empty,
            ThisPlayer,         //blue
            OtherPlayer,    //blue
            GameMessage,    //black
            PlayerAlert     //purple
        }

        /// <summary>
        /// The type of speaker that wrote the chat row.
        /// </summary>
        public RowType Type
        {
            get
            {
                if (_speakerType == RowType.Undetermined)
                    ChatRowSpeaker();
                return _speakerType;
            }
            private set
            {
                _speakerType = value;
            }
        }
        private RowType _speakerType;

        /// <summary>
        /// True if this chat row was spoken by this player or another player.
        /// </summary>
        public bool IsPlayer
        {
            get
            {
                if (!_isPlayerDetermined)
                    IsPlayer = PlayerChatRow();
                return _isPlayer;
            }
            private set
            {
                _isPlayer = value;
                _isPlayerDetermined = true;
            }
        }
        private bool _isPlayer;
        private bool _isPlayerDetermined;


        /// <summary>
        /// Image of this chat row.
        /// </summary>
        private Color[,] RowImage;

        /// <summary>
        /// Display name of the speaker of this chat row.
        /// </summary>
        public string SpeakerName
        {
            get
            {
                if (_speakerName == null)
                    _speakerName = GetSpeakerName();
                return _speakerName;
            }
            private set { _speakerName = value; }
        }
        private string _speakerName;

        /// <summary>
        /// Chat message.
        /// </summary>
        public string Message
        {
            get
            {
                if (_message == null)
                    _message = GetChatMessage();
                return _message;
            }
            private set { _message = value; }
        }
        private string _message;

        /// <summary>
        /// Display name of the player being controlled by this bot.
        /// </summary>
        public string PlayerName;

        /// <summary>
        /// The distance in pixels from the left edge (inclusive) of the chat row to the start of chat text (exclusive).
        /// </summary>
        private int SpeakerNameWidth
        {
            get
            {
                if (Type != RowType.OtherPlayer && Type != RowType.ThisPlayer)
                    return 0;
                return _speakerNameWidth;
            }
            set { _speakerNameWidth = value; }
        }
        private int _speakerNameWidth;

        /// <summary>
        /// Reads individual letters in the chat row.
        /// </summary>
        private LetterReader Reader;

        /// <summary>
        /// The maximum number of characters allowed for an in game name
        /// </summary>
        private const int MAX_NAME_LENGTH = 12;

        /// <summary>
        /// The number of pixel rows to look at when reading a chat row.
        /// Starts from the top pixel row of a chat row.
        /// We ignore the last 2 of 14 rows because apostrophes can overlap those.
        /// </summary>
        public const int ROWS_TO_READ = TextBoxTool.ROW_HEIGHT - 2;

        /// <summary>
        /// The height of a chat row in pixels.
        /// </summary>
        private const int ROW_HEIGHT = TextBoxTool.ROW_HEIGHT;

        /// <summary>
        /// Leftmost column where text can appear.
        /// </summary>
        private const int LEFT = 0;

        /// <summary>
        /// Minimum number of empty columns required for a space between words.
        /// </summary>
        private const int MIN_SPACE_WIDTH = 3;

        //Empty column value
        static readonly int EMPTY = 0;

        /// <summary>
        /// The moment at which this ChatRow was first read from the client and created.
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        /// Unique identifier for this ChatRow
        /// </summary>
        public int ID
        {
            get { return _id; }
            set
            {
                if (_id > 0)
                    throw new Exception("Illegal attempt to overwrite ID=" + _id + " with ID=" + value);
                _id = value;
            }        
        }
        private int _id;

        #endregion

        #region constructors


        public ChatRow() { }

        /// <summary>
        /// Creates a chat row based on an image of the row.
        /// </summary>
        /// <param name="rowImage">image of the chat row from the inside of the left border to the inside of the right scroll bar</param>
        /// <param name="playerName">the name of the player that the bot is controlling</param>
        public ChatRow(Color[,] rowImage, string playerName)
        {
            Time = DateTime.Now;
            RowImage = rowImage;
            PlayerName = playerName;
            Reader = LetterReader.GetInstance();
        }

        /// <summary>
        /// Creates a chat row using assumed values.
        /// </summary>
        /// <param name="type">the source of the chat row</param>
        /// <param name="playerName">name of the player being controlled by the bot</param>
        /// <param name="speakerName">name of the player speaking the row</param>
        /// <param name="message">message spoken on this row</param>
        public ChatRow(RowType type, string playerName, string speakerName, string message)
        {
            Time = DateTime.Now;
            Type = type;
            PlayerName = playerName;
            SpeakerName = speakerName;
            Message = message;
        }

        #endregion

        #region methods

        /// <summary>
        /// Determines the type of speaker for this chat row.
        /// </summary>
        /// <returns>The speaker type</returns>
        private void ChatRowSpeaker()
        {
            RowType speaker;

            if (IsPlayer)
            {
                speaker = OtherPlayer() ? RowType.OtherPlayer : RowType.ThisPlayer;
            }
            else
            {
                speaker = RowType.Unknown;
            }

            Type = speaker;
        }

        /// <summary>
        /// Determines if a row contains chat from a player (including the player).
        /// Searches for a colon marking the end of the speaker's display and records the width if found.
        /// </summary>
        /// <returns>True if the specified row contains chat from a player.</returns>
        private bool PlayerChatRow()
        {
            var chatTextColor = RGBExactFactory.PlayerChatText;

            for (int x = LEFT; x < RowImage.GetLength(0); x++)
            {
                for (int y = 0; y < ROWS_TO_READ; y++)
                {
                    if (chatTextColor.ColorInRange(RowImage[x, y]))
                    {
                        SpeakerNameWidth = x - 1;
                        IsPlayer = true;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if the first row of chat is another player's message.
        /// </summary>
        /// <returns>True if the first row of chat is another's player's text.</returns>
        private bool OtherPlayer()
        {
            return (IsPlayer && SpeakerName != PlayerName);
        }

        /// <summary>
        /// Gets the speaker name tag for this chat row.
        /// </summary>
        /// <returns>speaker name tag</returns>
        private string GetSpeakerName()
        {
            if (!IsPlayer)
                return "";

            Color[,] readableArea = ImageProcessing.ScreenPiece(RowImage, LEFT, RowImage.GetLength(0), 0, ROWS_TO_READ - 1);
            bool[,] nameTag = ImageProcessing.ColorFilter(readableArea, RGBExactFactory.PlayerChatName);
            string name = ReadText(nameTag);
            return name.Trim(' ', ':');
        }

        /// <summary>
        /// Gets the player chat message for this chat row.
        /// </summary>
        /// <returns>chat message string representation</returns>
        private string GetChatMessage()
        {
            if (!IsPlayer) { return ""; }

            Color[,] readableArea = ImageProcessing.ScreenPiece(RowImage, SpeakerNameWidth + 1, RowImage.GetLength(0), 0, ROWS_TO_READ - 1);
            bool[,] message = ImageProcessing.ColorFilter(readableArea, RGBExactFactory.PlayerChatText);
            return ReadText(message);
        }

        /// <summary>
        /// Translates a binary image of text into a string.
        /// </summary>
        /// <param name="text">binary image of text</param>
        /// <returns>translated text string</returns>
        private string ReadText(bool[,] text)
        {
            int blanks = 0;
            int[] columns = GetColumnValues(text);
            var letterColumns = new List<int>();
            var letters = new List<string>();

            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] == EMPTY)
                {
                    AddLetterColumns(letterColumns, letters);
                    blanks++;
                }
                else
                {
                    if (blanks >= MIN_SPACE_WIDTH)
                        letters.Add(" ");
                    blanks = 0;
                    letterColumns.Add(columns[i]);
                }
            }

            return string.Concat(letters);
        }

        /// <summary>
        /// Adds the letter represented by the list of letter columns to the list of letters.
        /// Does nothing if the columns do not match a recorded letter.
        /// </summary>
        /// <param name="letterColumns">list of column values that might represent a letter</param>
        /// <param name="letters">list of letters</param>
        private void AddLetterColumns(List<int> letterColumns, List<string> letters)
        {
            if (letterColumns.Count > 0)
            {
                letters.Add(Reader.ReadLetter(letterColumns));
                letterColumns.Clear();
            }
        }

        /// <summary>
        /// Adds a letter to the list if it is valid.
        /// </summary>
        /// <param name="letters">list of letters</param>
        /// <param name="letter">letter to add</param>
        private static void AddLetter(List<string> letters, string letter)
        {
            if (!string.IsNullOrEmpty(letter))
            {
                letters.Add(letter);
            }
        }

        /// <summary>
        /// Calculates the column values for a binary image of text.
        /// </summary>
        /// <param name="filteredImage">binary image of text</param>
        /// <returns>Column values for he image (left to right)</returns>
        private static int[] GetColumnValues(bool[,] filteredImage)
        {
            int columns = filteredImage.GetLength(0);
            int rows = filteredImage.GetLength(1);
            bool[] column = new bool[rows];
            int[] columnValues = new int[columns];

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    column[y] = filteredImage[x, y];
                }
                columnValues[x] = Letter.ColumnValue(column);
            }

            return columnValues;
        }

        /// <summary>
        /// Determines if another ChatRow is the same as this ChatRow.
        /// </summary>
        /// <param name="otherRow">the other ChatRow to check</param>
        /// <returns>True if both rows are identical</returns>
        public bool Equals(ChatRow otherRow)
        {
            return 
                Type == otherRow.Type &&
                SpeakerName == otherRow.SpeakerName &&
                Message == otherRow.Message;
        }

        /// <summary>
        /// Creates a dictionary containing the members 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetUrlDictionary()
        {
            var urlDictionary = new Dictionary<string, string>
            {
                { nameof(Message), Message },
                { nameof(PlayerName), PlayerName },
                { nameof(SpeakerName), SpeakerName },
                { nameof(Time), Time.ToString() },
                { nameof(Type), Type.ToString() }
            };
            return urlDictionary;
        }

        #endregion
    }
}
