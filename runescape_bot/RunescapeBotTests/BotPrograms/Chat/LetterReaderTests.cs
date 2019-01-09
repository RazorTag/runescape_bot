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
    public class LetterReaderTests
    {
        [TestMethod()]
        [DataRow("A", new int[] { 0b_0011_1111_1100, 0b_0100_1000_0000, 0b_1000_1000_0000, 0b_1000_1000_0000, 0b_0100_1000_0000, 0b_0011_1111_1100 })]
        [DataRow("a", new int[] { 0b_0000_0101_1000, 0b_0000_1010_0100, 0b_0000_1010_0100, 0b_0000_1010_0100, 0b_0000_0111_1100 })]
        public void ReadLetterTest(string letter, int[] pixelColumns)
        {
            LetterReader reader = LetterReader.GetInstance();
            string readLetter = reader.ReadLetter(new List<int>(pixelColumns));
            Assert.AreEqual(letter, readLetter);
        }
    }
}