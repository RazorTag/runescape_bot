using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace RunescapeBot.BotPrograms.Chat.Tests
{
    [TestClass()]
    public class LetterReaderTests
    {
        /// <summary>
        /// Column pixels for the letters being tested
        /// </summary>
        private readonly int[][] columnPixels = new int[][]
        {
            Letters.A,
            Letters.a,
            Letters.openCloseParenthesis,
            Letters.question
        };

        [TestMethod()]
        [DataRow("A", 0)]
        [DataRow("a", 1)]
        [DataRow("()", 2)]
        [DataRow("?", 3)]
        public void ReadLetterTest(string letter, int columnPixelIndex)
        {
            LetterReader reader = LetterReader.GetInstance();
            string readLetter = reader.ReadLetter(new List<int>(columnPixels[columnPixelIndex]));
            Assert.AreEqual(letter, readLetter);
        }
    }
}