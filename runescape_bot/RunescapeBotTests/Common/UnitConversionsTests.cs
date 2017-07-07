using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunescapeBot.Common.Tests
{
    [TestClass()]
    public class UnitConversionsTests
    {
        [TestMethod()]
        [DataRow(0, 0)]
        [DataRow(55, 3300000)]
        public void MinutesToMillisecondsTest(double minutes, int expectedMS)
        {
            int actualMS = UnitConversions.MinutesToMilliseconds(minutes);
            Assert.AreEqual(actualMS, expectedMS);
        }

        [TestMethod()]
        [DataRow(0, 0)]
        [DataRow(32, 115200000)]
        public void HoursToMillisecondsTest(double hours, int expectedMS)
        {
            int actualMS = UnitConversions.HoursToMilliseconds(hours);
            Assert.AreEqual(actualMS, expectedMS);
        }
    }
}