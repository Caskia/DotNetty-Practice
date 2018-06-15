using System;

namespace Echo.Codecs
{
    [Serializable]
    public class Request
    {
        public byte[] Body { get; set; }

        public short Code { get; set; }
    }
}