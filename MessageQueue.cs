using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Base
{
    public class Message
    {
        public DateTime Timestamp { get; }
        public string Content { get; }

        public Message(string content)
        {
            Timestamp = DateTime.Now;
            Content = content;
        }

        public override string ToString()
        {
            return $"[{Timestamp.ToString("yyyy-MM-dd HH:mm:ss")}] {Content}";
        }

        public static implicit operator Message(string content) => new Message(content);
    }
}
