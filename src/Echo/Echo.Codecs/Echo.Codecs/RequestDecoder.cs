using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.IO;

namespace Echo.Codecs
{
    public class RequestDecoder : ByteToMessageDecoder
    {
        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            int length = message.ReadableBytes;
            if (length <= 0)
            {
                return;
            }

            Stream inputStream = null;
            try
            {
                var bytes = new byte[message.ReadableBytes];
                message.ReadBytes(bytes);

                var decoded = bytes.ToObject();

                output.Add(decoded);
            }
            catch (Exception exception)
            {
                throw new CodecException(exception);
            }
            finally
            {
                inputStream?.Dispose();
            }
        }
    }
}