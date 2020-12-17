using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Antonkaster.MqWorks
{
    public class MqConnectionConfig : IMqConnectionConfig
    {
        public string Host { get; set; } = "localhost";
        public string VHost { get; set; } = "/";
        public string User { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
}
