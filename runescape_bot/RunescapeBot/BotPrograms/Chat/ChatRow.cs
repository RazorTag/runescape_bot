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
        /// The color of a player name tag
        /// </summary>
        public Color NameColor = Color.Black;

        /// <summary>
        /// The different "speakers" for a line of text.
        /// </summary>
        public enum ChatRowType
        {
            Unknown,
            Empty,
            Player,         //blue
            OtherPlayer,    //blue
            GameMessage,    //black
            PlayerAlert     //purple
        }

        /// <summary>
        /// The type of speaker that wrote the chat row.
        /// </summary>
        public ChatRowType SpeakerType;

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
        /// Display name of the player being controlled by this bot.
        /// </summary>
        public string PlayerName;

        /// <summary>
        /// The distance in pixels from the left edge (inclusive) of the chat row to the start of chat text (exclusive).
        /// </summary>
        private int SpeakerNameWidth;

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
        private const int LEFT = 4;

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
        private ChatRowType ChatRowSpeaker()
        {
            if (PlayerChatRow())
            {
                GetSpeakerName();
                return OtherPlayer() ? ChatRowType.OtherPlayer : ChatRowType.Player;
            }

            return ChatRowType.Unknown;
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
            return SpeakerName == PlayerName;
        }

        /// <summary>
        /// Gets the speaker name tag for this chat row.
        /// </summary>
        /// <returns>speaker name tag</returns>
        private string GetSpeakerName()
        {
            Color[,] readableArea = ImageProcessing.ScreenPiece(RowImage, LEFT, RowImage.Length - 1, 0, ROWS_TO_READ - 1);
            bool[,] nameTag = ImageProcessing.ColorFilter(readableArea, RGBExactFactory.PlayerChatName);
            string name = ReadText(nameTag);

            name.Trim(' ', ':');
            return name;
        }

        /// <summary>
        /// Translates a binary image of text into a string.
        /// </summary>
        /// <param name="text">binary image of text</param>
        /// <returns>translated text string</returns>
        private string ReadText(bool[,] text)
        {
            return "";  //TODO
        }

        #endregion
    }
}
