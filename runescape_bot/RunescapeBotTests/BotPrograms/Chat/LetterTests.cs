using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunescapeBot.BotPrograms.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Chat.Tests
{
    [TestClass()]
    public class LetterTests
    {
        [TestMethod()]
        [DataRow(new bool[] { false }, 0)]
        [DataRow(new bool[] { true, false, true, true, false }, 13)]
        [DataRow(new bool[] { false, false, false, false, false, false }, 0)]
        [DataRow(new bool[] { true, true, true, true }, 15)]
        public void ColumnValueTest(bool[] column, int expected)
        {
            int value = Letter.ColumnValue(column);
            Assert.AreEqual(value, expected);
        }
    }
}