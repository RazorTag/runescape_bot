using ChatServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatServer.Controllers
{
    public class ChatController : ApiController
    {
        private ChatDbContext db = new ChatDbContext();

        // GET api/chat
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/chat/5
        public string Get(int id)
        {
            return id.ToString();
        }

        // POST api/chat
        public void Post([FromBody]string value)
        {
        }

        // PUT api/chat/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/chat/5
        public void Delete(int id)
        {
        }
    }
}
