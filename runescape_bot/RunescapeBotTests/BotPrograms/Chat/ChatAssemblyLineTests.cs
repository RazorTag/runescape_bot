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
    public class ChatAssemblyLineTests
    {
        //[TestMethod()]
        //public void ChatAssemblyLineTest()
        //{
        //    ChatRow row1, row2, row3;
        //    Response response1, response2;
        //    string responseMessage;
        //    var assemblyLine = new MessageQueues();

        //    row1 = new ChatRow(ChatRow.RowType.OtherPlayer, "Player One", "Other Player", "Are you a bot?");
        //    assemblyLine.NewComment(row1);
        //    assemblyLine.NextComment(out row2);
        //    Assert.IsTrue(row2.Equals(row1));
        //    response1 = new Response(1, "nope");
        //    assemblyLine.ResponseReceived(response1);
        //    assemblyLine.GetResponses(out responseMessage);
        //    Assert.AreEqual(responseMessage, response1.Message);

        //    row1 = new ChatRow(ChatRow.RowType.OtherPlayer, "Player One", "Other Player", "Really?");
        //    row2 = new ChatRow(ChatRow.RowType.OtherPlayer, "Player Two", "Other Player", "Are you sure?");
        //    assemblyLine.NewComment(row1);
        //    assemblyLine.NewComment(row2);
        //    assemblyLine.NextComment(out row3);
        //    Assert.IsTrue(row3.Equals(row1));
        //    assemblyLine.NextComment(out row3);
        //    Assert.IsTrue(row3.Equals(row2));
        //    response1 = new Response(2, "indeed not");
        //    response2 = new Response(3, "still no");
        //    assemblyLine.ResponseReceived(response1);
        //    assemblyLine.ResponseReceived(response2);
        //    assemblyLine.GetResponses(out responseMessage);
        //    Assert.AreEqual(responseMessage, response1.Message);
        //    assemblyLine.GetResponses(out responseMessage);
        //    Assert.AreEqual(responseMessage, response2.Message);
        //}
    }
}