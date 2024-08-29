using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSCSRPG.Core
{
    public class MessageBroker
    {
        private static readonly MessageBroker s_messageBroker = new MessageBroker();

        private MessageBroker() { }

        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        public static MessageBroker GetInstance()
        {
            return s_messageBroker;
        }

        public void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}
