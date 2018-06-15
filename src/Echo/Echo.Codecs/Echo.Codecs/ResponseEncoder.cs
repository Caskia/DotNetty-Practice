using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Echo.Codecs
{
    public class ResponseEncoder : MessageToByteEncoder<Response>
    {
        protected override void Encode(IChannelHandlerContext context, Response message, IByteBuffer output)
        {
            if (message != null)
            {
                output.WriteBytes(message.ToByteArray());
            }
        }
    }
}