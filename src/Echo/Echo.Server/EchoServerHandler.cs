using DotNetty.Transport.Channels;
using Echo.Codecs;
using System;
using System.Text;

namespace Echo.Server
{
    public class EchoServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message != null)
            {
                var request = message as Request;

                Console.WriteLine("Received from client: " + Encoding.UTF8.GetString(request.Body));
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }

        //public override void ChannelRead(IChannelHandlerContext context, object message)
        //{
        //    if (message != null)
        //    {
        //        Console.WriteLine("Received from client: " + message.ToString());
        //    }
        //}

        //public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        //{
        //    Console.WriteLine("Exception: " + exception);
        //    context.CloseAsync();
        //}
    }
}