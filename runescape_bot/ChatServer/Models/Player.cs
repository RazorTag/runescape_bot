using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatServer.Models
{
    public class Player
    {
        [Key]
        public string PlayerName { get; set; }

        public virtual List<Conversation> Conversations { get; set; }
    }
}