using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System.Text;

namespace Echo.Codecs
{
    public class StringEncoder : MessageToByteEncoder<string>
    {
        protected override void Encode(IChannelHandlerContext context, string message, IByteBuffer output)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                output.WriteBytes(messageBytes);
            }
        }
    }
}