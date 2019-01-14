using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunescapeBot.ImageTools;
using System.Drawing;

namespace RunescapeBot.BotPrograms.Chat.Tests
{
    [TestClass()]
    public class ConversationTests
    {
        #region conversation 1

        private readonly ChatRowTest ChatRowTest1 = new ChatRowTest("BioGalt", "Line 1", "BioGalt", ChatRow.ChatRowType.ThisPlayer, true);
        private readonly ChatRowTest ChatRowTest2 = new ChatRowTest("BioGalt", "Second me?", "BioGalt", ChatRow.ChatRowType.ThisPlayer, true);
        private readonly ChatRowTest ChatRowTest3 = new ChatRowTest("BioGalt", "Hello World!", "BioGalt", ChatRow.ChatRowType.ThisPlayer, true);
        private readonly ChatRowTest ChatRowTest4 = new ChatRowTest("BioGalt", "Ridiculous &%#%&", "BioGalt", ChatRow.ChatRowType.ThisPlayer, true);
        private readonly ChatRowTest NonPlayerChatRow = new ChatRowTest("", "", "BioGalt", ChatRow.ChatRowType.Unknown, false);

        [TestMethod()]
        public void ConversationTest1()
        {
            GameScreen screen = new GameScreen(null, null);
            TextBoxTool textBox = new TextBoxTool(null, screen);
            Conversation conversation = new Conversation(textBox, screen, false);

            screen.Value = ScreenScraper.GetRGB(RunescapeBotTests.Properties.Resources.full_1);
            conversation.ReadChatRows();
            TestChatRow(conversation.ChatRows[0], ChatRowTest2);
            TestChatRow(conversation.ChatRows[1], ChatRowTest1);
            for (int i = 2; i < Conversation.CHAT_ROW_COUNT; i++)
                Assert.IsFalse(conversation.ChatRows[i].IsPlayer);

            screen.Value = ScreenScraper.GetRGB(RunescapeBotTests.Properties.Resources.full_2);
            conversation.ReadChatRows();
            TestChatRow(conversation.ChatRows[0], ChatRowTest3);
            TestChatRow(conversation.ChatRows[1], ChatRowTest2);
            TestChatRow(conversation.ChatRows[2], ChatRowTest1);

            //TODO test scroll match

            screen.Value = ScreenScraper.GetRGB(RunescapeBotTests.Properties.Resources.full_3);
            conversation.ReadChatRows();
            TestChatRow(conversation.ChatRows[0], ChatRowTest4);
            TestChatRow(conversation.ChatRows[1], ChatRowTest3);
            TestChatRow(conversation.ChatRows[2], ChatRowTest2);
            TestChatRow(conversation.ChatRows[3], ChatRowTest1);

            //TODO test scroll match
        }

        private void TestChatRow(ChatRow chatRow, ChatRowTest test)
        {
            Assert.AreEqual(test.SpeakerName, chatRow.SpeakerName);
            Assert.AreEqual(test.Message, chatRow.Message);
            Assert.AreEqual(test.RowType, chatRow.Type);
        }

        #endregion

        private class ChatRowTest
        {
            public string SpeakerName;
            public string Message;
            public string PlayerName;
            public ChatRow.ChatRowType RowType;
            public bool IsPlayer;

            public ChatRowTest(string speakerName, string message, string playerName, ChatRow.ChatRowType rowType, bool isPlayer)
            {
                SpeakerName = speakerName;
                Message = message;
                PlayerName = playerName;
                RowType = rowType;
                IsPlayer = isPlayer;
            }
        }
    }
}