using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autabee.RosScout.Components
{
    public class LogMessage
    {
        public readonly LogEvent message;
        public LogMessage()
        {

        }
        public LogMessage(LogEvent message)
        {
            this.message = message;
            MultiLine = message.RenderMessage().Count(c => c == '\n') >= 1;
        }
        public bool MultiLine;
        public bool ShowFullMessage;
        public string MessageClass => ShowFullMessage ? "log-message" : "log-message-short";
    }
}
