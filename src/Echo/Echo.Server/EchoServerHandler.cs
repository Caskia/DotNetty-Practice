using DotNetty.Transport.Channels;
using System;

namespace Echo.Server
{
    public class EchoServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message != null)
            {
                Console.WriteLine("Received from client: " + message.ToString());
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}