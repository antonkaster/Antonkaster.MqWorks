using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Antonkaster.MqWorks.Extensions
{
    public class MqRpcResponseTimeout : Exception
    {
        public MqRpcResponseTimeout() : base("MQ RPC response timeout")
        {
        }

        public MqRpcResponseTimeout(string message) : base("MQ RPC response timeout: " + message)
        {
        }

        public MqRpcResponseTimeout(string message, Exception innerException) : base("MQ RPC response timeout: " + message, innerException)
        {
        }
    }
}
