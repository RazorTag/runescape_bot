using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunescapeBot.ImageTools;
using System.Drawing;

namespace RunescapeBot.BotPrograms.Chat.Tests
{
    [TestClass()]
    public class ChatRowTests
    {
        /// <summary>
        /// Images of chat rows for testing.
        /// </summary>
        private readonly Bitmap[] rowImages = new Bitmap[]
        {
            RunescapeBotTests.Properties.Resources.capital_letters,
            RunescapeBotTests.Properties.Resources.lowercase_letters,
            RunescapeBotTests.Properties.Resources.digits,
            RunescapeBotTests.Properties.Resources.punctuation_and_symbols,
            RunescapeBotTests.Properties.Resources.non_player_row
        };

        [TestMethod()]
        [DataRow(0, "BioGalt", "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z", "OtherPlayer", ChatRow.RowType.OtherPlayer, true)]
        [DataRow(1, "BioGalt", "~abcdefghijklmnopqrstuvwxyz", "BioGalt", ChatRow.RowType.ThisPlayer, true)]
        [DataRow(2, "BioGalt", "0123456789", "BioGalt", ChatRow.RowType.ThisPlayer, true)]
        [DataRow(3, "BioGalt", "`~!@#$%^&*()_-+={}[]:;\'<>,.?/\\", "BioGalt", ChatRow.RowType.ThisPlayer, true)]
        [DataRow(4, "", "", "", ChatRow.RowType.Unknown, false)]
        public void ChatRowTest(int rowImageIndex, string speakerName, string text, string playerName, ChatRow.RowType rowType, bool isPlayer)
        {
            Color[,] rowImage = ScreenScraper.GetRGB(rowImages[rowImageIndex]);
            ChatRow chatRow = new ChatRow(rowImage, playerName);
            Assert.AreEqual(speakerName, chatRow.SpeakerName);
            Assert.AreEqual(text, chatRow.Message);
            Assert.AreEqual(rowType, chatRow.Type);
            Assert.AreEqual(isPlayer, chatRow.IsPlayer);
        }
    }
}