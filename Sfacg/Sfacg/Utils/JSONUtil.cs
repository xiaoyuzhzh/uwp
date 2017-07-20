using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Utils
{
    class JSONUtil
    {

        public static string serialize<T>(T t)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, t);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            string dataString = Encoding.UTF8.GetString(dataBytes);
            return dataString;
        }

        public static T deSerialize<T>(string value)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(value));
            try
            {
                T data = (T)serializer.ReadObject(ms);
                return data;
            }
            catch (Exception)
            {
            }
            return default(T);
        }
    }
}
