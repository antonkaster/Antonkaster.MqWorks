using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Antonkaster.MqWorks.Exceptions
{
    public class MqRecieveException : Exception
    {
        public MqRecieveException() : base("Recieve error")
        {
        }

        public MqRecieveException(string message) : base("Recieve error: " + message)
        {
        }

        public MqRecieveException(string message, Exception innerException) : base("Recieve error: " + message, innerException)
        {
        }

    }
}
