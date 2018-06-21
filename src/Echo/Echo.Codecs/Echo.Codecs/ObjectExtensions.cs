using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Echo.Codecs
{
    public static class ObjectExtensions
    {
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object ToObject(this byte[] data)
        {
            if (data == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                return bf.Deserialize(ms);
            }
        }
    }
}