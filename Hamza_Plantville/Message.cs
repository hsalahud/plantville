using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamza_Plantville
{
    public class Message
    {
        /// <summary>
        /// message property
        /// </summary>
        public string username { get; set; }
        public string message { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        /// <summary>
        /// message constructor
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="user"></param>
        public Message(string msg, string user)
        {
            this.message = msg;
            this.username = user;
        }
    }
}
