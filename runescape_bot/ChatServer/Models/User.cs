using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatServer.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }

        public string password { get; set; }

        public string PlayerAccess { get; set; }
    }
}