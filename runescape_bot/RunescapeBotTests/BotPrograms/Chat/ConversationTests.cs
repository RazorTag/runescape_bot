using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunescapeBot.ImageTools;
using RunescapeBot.UITools;
using System.Drawing;

namespace RunescapeBot.BotPrograms.Chat.Tests
{
    [TestClass()]
    public class ConversationTests
    {
        #region properties

        private GameScreen screen;
        private TextBoxTool textBox;
        private RunParams runParams;
        private RSClient client;
        private Keyboard keyboard;
        private Conversation conversation;

        private readonly ChatRowTest ChatRowTest1 = new ChatRowTest("BioGalt", "Line 1", "BioGalt", ChatRow.RowType.ThisPlayer, true);
        private readonly ChatRowTest ChatRowTest2 = new ChatRowTest("BioGalt", "Second me?", "BioGalt", ChatRow.RowType.ThisPlayer, true);
        private readonly ChatRowTest ChatRowTest3 = new ChatRowTest("BioGalt", "Hello World!", "BioGalt", ChatRow.RowType.ThisPlayer, true);
        private readonly ChatRowTest ChatRowTest4 = new ChatRowTest("BioGalt", "Ridiculous &%#%&", "BioGalt", ChatRow.RowType.ThisPlayer, true);

        private ChatRowTest[] ChatRowOther = new ChatRowTest[]
        {
            new ChatRowTest("Ovast", "@@@infernoscape.C 0m@@@.Cheap.Teamviewer.Infernal.Capes.0Bans", "BioGalt", ChatRow.RowType.OtherPlayer, true),
            new ChatRowTest("Coffermity", "50 mil from what?", "BioGalt", ChatRow.RowType.OtherPlayer, true),
            new ChatRowTest("Bzm1213", "How much do i need", "BioGalt", ChatRow.RowType.OtherPlayer, true),
            new ChatRowTest("NickReady", "I did quest for mm2", "BioGalt", ChatRow.RowType.OtherPlayer, true),
            new ChatRowTest("", "", "BioGalt", ChatRow.RowType.Unknown, false),
            new ChatRowTest("Ovast", "@@@infernoscape.C 0m@@@.Cheap.Teamviewer.Infernal.Capes.0Bans", "BioGalt", ChatRow.RowType.OtherPlayer, true),
            new ChatRowTest("NickReady", "Then i made about 50 mil in", "BioGalt", ChatRow.RowType.OtherPlayer, true),
            new ChatRowTest("Coffermity", "So youve made all your money doing those things youve said", "BioGalt", ChatRow.RowType.OtherPlayer, true)
        };
    
        #endregion

        private void SetUpConversationTests()
        {
            screen = new GameScreen(null, null);
            textBox = new TextBoxTool(null, screen);
            runParams = new RunParams();
            client = new RSClient(runParams);
            keyboard = new Keyboard(client);
            conversation = new Conversation(textBox, screen, keyboard, false);
        }

        #region tests

        [TestMethod()]
        public void ConversationTestSelf()
        {
            SetUpConversationTests();

            screen.Value = ScreenScraper.GetRGB(RunescapeBotTests.Properties.Resources.full_1);
            conversation.ReadChatRows();
            Assert.AreEqual(ChatRowTest1.PlayerName, conversation.PlayerName);
            TestChatRow(conversation.ChatRows[0], ChatRowTest2);
            TestChatRow(conversation.ChatRows[1], ChatRowTest1);
            for (int i = 2; i < Conversation.CHAT_ROW_COUNT; i++)
                Assert.IsFalse(conversation.ChatRows[i].IsPlayer);

            screen.Value = ScreenScraper.GetRGB(RunescapeBotTests.Properties.Resources.full_2);
            conversation.ReadChatRows();
            Assert.AreEqual(conversation.NewChatLines, 1);
            TestChatRow(conversation.ChatRows[0], ChatRowTest3);
            TestChatRow(conversation.ChatRows[1], ChatRowTest2);
            TestChatRow(conversation.ChatRows[2], ChatRowTest1);

            screen.Value = ScreenScraper.GetRGB(RunescapeBotTests.Properties.Resources.full_3);
            conversation.ReadChatRows();
            Assert.AreEqual(conversation.NewChatLines, 1);
            TestChatRow(conversation.ChatRows[0], ChatRowTest4);
            TestChatRow(conversation.ChatRows[1], ChatRowTest3);
            TestChatRow(conversation.ChatRows[2], ChatRowTest2);
            TestChatRow(conversation.ChatRows[3], ChatRowTest1);
        }

        [TestMethod()]
        public void ConversationTestOtherPlayer()
        {
            SetUpConversationTests();

            screen.Value = ScreenScraper.GetRGB(RunescapeBotTests.Properties.Resources.full_other_players);
            conversation.ReadChatRows();
            Assert.AreEqual(ChatRowOther[0].PlayerName, conversation.PlayerName);
            for (int i = 0; i < ChatRowOther.Length; i++)
                TestChatRow(conversation.ChatRows[i], ChatRowOther[i]);
        }

        private void TestChatRow(ChatRow chatRow, ChatRowTest test)
        {
            Assert.AreEqual(test.PlayerName, chatRow.PlayerName);
            Assert.AreEqual(test.SpeakerName, chatRow.SpeakerName);
            Assert.AreEqual(test.Message, chatRow.Message);
            Assert.AreEqual(test.RowType, chatRow.Type);
        }

        #endregion

        public class ChatRowTest
        {
            public string SpeakerName;
            public string Message;
            public string PlayerName;
            public ChatRow.RowType RowType;
            public bool IsPlayer;

            public ChatRowTest(string speakerName, string message, string playerName, ChatRow.RowType rowType, bool isPlayer)
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