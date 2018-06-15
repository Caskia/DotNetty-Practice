using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System.Collections.Generic;
using System.Text;

namespace Echo.Codecs
{
    public class StringDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            if (input != null)
            {
                output.Add(input.ToString(Encoding.UTF8));
            }
        }
    }
}