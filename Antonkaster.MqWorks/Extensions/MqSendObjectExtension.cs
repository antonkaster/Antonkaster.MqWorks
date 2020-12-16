using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonkaster.MqWorks.Extensions
{
    public static class MqSendObjectExtension
    {
        public static byte[] ToBytes<T>(this T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Object can't be null!");
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
        }

        public static T ToObject<T>(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("Bytes can't be null!");
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
        }
    }
}
