using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunescapeBot.BotPrograms.Chat;
using RunescapeBot.ImageTools;
using RunescapeBot.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.Networking.Tests
{
    [TestClass()]
    public class HttpInstanceTests
    {
        [TestMethod()]
        public async Task JsonPostTestAsync()
        {
            var values = new Dictionary<string, string>
            {
                { "email", "peter@klaven" },
                { "password", "cityslicka" }
            };

            var response = await HttpInstance.Instance.JsonPost("https://reqres.in/api/login", new FormUrlEncodedContent(values));
            Assert.IsTrue(response.Contains("QpwL5tke4Pnpja7X"));
        }
    }
}