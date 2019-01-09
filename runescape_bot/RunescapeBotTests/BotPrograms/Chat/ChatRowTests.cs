﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunescapeBot.BotPrograms.Chat;
using RunescapeBot.FileIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunescapeBot.BotPrograms.Chat.Tests
{
    [TestClass()]
    public class ChatRowTests
    {
        private readonly Bitmap[] rowImages = new Bitmap[]
            {
                RunescapeBotTests.Properties.Resources.capital_letters,
                RunescapeBotTests.Properties.Resources.lowercase_letters
            };

        [TestMethod()]
        [DataRow(0, "BioGalt", "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z", "OtherPlayer", ChatRow.ChatRowType.OtherPlayer)]
        [DataRow(0, "BioGalt", "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z", "BioGalt", ChatRow.ChatRowType.ThisPlayer)]
        [DataRow(1, "BioGalt", "~abcdefghijklmnopqrstuvwxyz", "BioGalt", ChatRow.ChatRowType.ThisPlayer)]
        public void ChatRowTest(int rowImageIndex, string speakerName, string text, string playerName, ChatRow.ChatRowType rowType)
        {
            Color[,] rowImage = DebugUtilities.BitmapToColorArray(rowImages[0]);
            ChatRow chatRow = new ChatRow(rowImage, playerName);
            Assert.AreEqual(speakerName, chatRow.SpeakerName);
            Assert.AreEqual(text, chatRow.Message);
            Assert.AreEqual(rowType, chatRow.Type);
        }
    }
}