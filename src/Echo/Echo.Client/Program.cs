using DotNetty.Buffers;
using DotNetty.Codecs.Mqtt;
using DotNetty.Common.Internal.Logging;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Echo.Codecs;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Echo.Client
{
    internal class Program
    {
        public static IPEndPoint GetIPEndPointFromHostName(string hostName, int port, AddressFamily? addressFamily = AddressFamily.InterNetwork, bool throwIfMoreThanOneIP = true)
        {
            var ipAddresses = Dns.GetHostAddresses(hostName);

            if (addressFamily.HasValue)
            {
                ipAddresses = ipAddresses
                    .Where(a => a.AddressFamily == addressFamily.Value)
                    .ToArray();
            }

            if (ipAddresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName"
                );
            }
            else if (throwIfMoreThanOneIP && ipAddresses.Length > 1)
            {
                throw new ArgumentException(
                   "There is more that one IP address to the specified host.",
                   "hostName"
               );
            }

            return new IPEndPoint(ipAddresses.FirstOrDefault(), port);
        }

        private static void Main(string[] args) => RunClientAsync().Wait();

        private static async Task RunClientAsync()
        {
            InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));

            var group = new MultithreadEventLoopGroup();

            var bootstrap = new Bootstrap();

            try
            {
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;

                        pipeline.AddLast(new LoggingHandler());
                        //pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        //pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                        //pipeline.AddLast("mqtt-encoder", new MqttEncoder());
                        //pipeline.AddLast("mqtt-decoder", new MqttDecoder(true, 256 * 1024));
                        //pipeline.AddLast("string-encoder", new StringEncoder());
                        //pipeline.AddLast("string-decoder", new StringDecoder());
                        pipeline.AddLast("request-encoder", new RequestEncoder());
                        pipeline.AddLast("request-decoder", new RequestDecoder());
                        pipeline.AddLast("echo", new EchoClientHandler());
                    }));

                //var clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998));

                var clientChannel = await bootstrap.ConnectAsync(GetIPEndPointFromHostName("localhost", 9998, AddressFamily.InterNetwork, false));

                while (true)
                {
                    var readContent = Console.ReadLine();
                    if (readContent == "exit")
                    {
                        break;
                    }

                    //await clientChannel.WriteAndFlushAsync(Unpooled.Buffer().WriteBytes(Encoding.UTF8.GetBytes(readContent)));

                    await clientChannel.WriteAndFlushAsync(new Request() { Code = 1, Body = Encoding.UTF8.GetBytes(readContent) });
                }

                await clientChannel.CloseAsync();
            }
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }
    }
}