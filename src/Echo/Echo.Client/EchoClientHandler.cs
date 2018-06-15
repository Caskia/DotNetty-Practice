using DotNetty.Transport.Channels;
using Echo.Codecs;
using System;
using System.Text;

namespace Echo.Client
{
    public class EchoClientHandler : ChannelHandlerAdapter
    {
        public EchoClientHandler()
        {
        }

        public override void ChannelActive(IChannelHandlerContext context)
            => context.WriteAndFlushAsync(new Request() { Code = 1, Body = Encoding.UTF8.GetBytes("ping") });

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message != null)
            {
                var request = message as Request;

                Console.WriteLine("Received from server: " + Encoding.UTF8.GetString(request.Body));
            }
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }

        //public override void ChannelActive(IChannelHandlerContext context)
        //   => context.WriteAndFlushAsync("ping");

        //public override void ChannelRead(IChannelHandlerContext context, object message)
        //{
        //    if (message != null)
        //    {
        //        Console.WriteLine("Received from server: " + message.ToString());
        //    }
        //}

        //public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        //{
        //    Console.WriteLine("Exception: " + exception);
        //    context.CloseAsync();
        //}
    }
}