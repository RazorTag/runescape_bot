using RunescapeBot.Common;
using RunescapeBot.ImageTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Display name of the player.
        /// </summary>
        public string PlayerName;

        /// <summary>
        /// The width in pixels from the left edge (inclusive) of the chat row to the colon after the player's name (exclusive).
        /// </summary>
        private int SpeakerNameWidth;

        /// <summary>
        /// The maximum number of characters allowed for an in game name
        /// </summary>
        private const int _maxNameLength = 12;

        /// <summary>
        /// The height of a chat row in pixels.
        /// </summary>
        private const int ROW_HEIGHT = TextBoxTool.ROW_HEIGHT;

        #endregion

        #region constructors

        public ChatRow(Color[,] rowImage, string playerName)
        {
            RowImage = rowImage;
            PlayerName = playerName;
        }

        #endregion

        #region methods

        private ChatRowType ChatRowSpeaker()
        {
            if (PlayerChatRow())
            {
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
            //TODO
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
            return "";
        }

        #endregion
    }
}
