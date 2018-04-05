using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunescapeBot.BotPrograms.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.BotPrograms.Popups.Tests
{
    [TestClass()]
    public class RightClickInventoryTests
    {
        [TestMethod()]
        [DataRow(1, null, 1)]
        [DataRow(1, new int[1] { 0 }, 2)]
        [DataRow(2, new int[3] { 0, 2, 5 }, 4)]
        public void OptionIndexTest(int normalIndex, int[] extraOptions, int expected)
        {
            int actual = RightClickInventory.OptionIndex(normalIndex, extraOptions);
            Assert.AreEqual(expected, actual);
        }
    }
}