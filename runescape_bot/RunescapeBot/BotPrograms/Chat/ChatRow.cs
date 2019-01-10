using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using RunescapeBot.ImageTools.Filters.FilterFactories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms.Chat
{
    public class ChatRow
    {
        #region properties

        /// <summary>
        /// The different "speakers" for a line of text.
        /// </summary>
        public enum ChatRowType
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
        public ChatRowType Type
        {
            get
            {
                if (_speakerType == ChatRowType.Undetermined)
                    ChatRowSpeaker();
                return _speakerType;
            }
            private set
            {
                _speakerType = value;
            }
        }
        private ChatRowType _speakerType;

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
                if (string.IsNullOrEmpty(_speakerName))
                    _speakerName = GetSpeakerName();
                return _speakerName;
            }
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
                    _message = GetMessage();
                return _message;
            }
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
                if (Type != ChatRowType.OtherPlayer && Type != ChatRowType.ThisPlayer)
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

        #endregion

        #region constructors

        /// <summary>
        /// Creates a chat row.
        /// </summary>
        /// <param name="rowImage">image of the chat row from the inside of the left border to the inside of the right scroll bar</param>
        /// <param name="playerName">the name of the player that the bot is controlling</param>
        public ChatRow(Color[,] rowImage, string playerName)
        {
            RowImage = rowImage;
            PlayerName = playerName;
            Reader = LetterReader.GetInstance();
        }

        #endregion

        #region methods

        /// <summary>
        /// Determines the type of speaker for this chat row.
        /// </summary>
        /// <returns>The speaker type</returns>
        private void ChatRowSpeaker()
        {
            ChatRowType speaker;

            if (PlayerChatRow())
            {
                speaker = OtherPlayer() ? ChatRowType.OtherPlayer : ChatRowType.ThisPlayer;
            }
            else
            {
                speaker = ChatRowType.Unknown;
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
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if the first row of chat is another player's message.
        /// Assumes that this row is spoken by either this player or another player.
        /// </summary>
        /// <returns>True if the first row of chat is another's player's text.</returns>
        private bool OtherPlayer()
        {
            return SpeakerName != PlayerName;
        }

        /// <summary>
        /// Gets the speaker name tag for this chat row.
        /// </summary>
        /// <returns>speaker name tag</returns>
        private string GetSpeakerName()
        {
            Color[,] readableArea = ImageProcessing.ScreenPiece(RowImage, LEFT, RowImage.GetLength(0), 0, ROWS_TO_READ - 1);
            bool[,] nameTag = ImageProcessing.ColorFilter(readableArea, RGBExactFactory.PlayerChatName);
            string name = ReadText(nameTag);
            return name.Trim(' ', ':');
        }

        /// <summary>
        /// Gets the chat message for this chat row.
        /// </summary>
        /// <returns>chat message string representation</returns>
        private string GetMessage()
        {
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

        #endregion
    }
}
