using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatCore
{
    public class Message
    {
        public Message(Users sender , string text)
        {
            Sender = sender;
            Text = text;
        }
        public string Text { get; set; }    
        public Users Sender { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;    
    }
}
