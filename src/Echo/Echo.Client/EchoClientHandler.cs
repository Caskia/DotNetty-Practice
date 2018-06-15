using DotNetty.Transport.Channels;
using System;

namespace Echo.Client
{
    public class EchoClientHandler : ChannelHandlerAdapter
    {
        public EchoClientHandler()
        {
        }

        public override void ChannelActive(IChannelHandlerContext context)
            => context.WriteAndFlushAsync("ping");

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message != null)
            {
                Console.WriteLine("Received from server: " + message.ToString());
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}