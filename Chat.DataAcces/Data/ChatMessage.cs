using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DataAcces.Data
{
    public class ChatMessage
    {
        public  long Id { get; set; }
        public string message { get; set; } = string.Empty;
        public string user_name { get; set; } = string.Empty;
    }
}
