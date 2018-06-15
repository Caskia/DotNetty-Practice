using DotNetty.Codecs;
using DotNetty.Codecs.Mqtt;
using DotNetty.Common.Internal.Logging;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Threading.Tasks;

namespace Echo.Server
{
    internal class Program
    {
        private static void Main(string[] args) => RunServerAsync().Wait();

        private static async Task RunServerAsync()
        {
            InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));

            var dispatcher = new DispatcherEventLoopGroup();
            var bossGroup = dispatcher;
            var workerGroup = new WorkerEventLoopGroup(dispatcher);

            var bootstrap = new ServerBootstrap();

            try
            {
                bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler("SRV-LSTN"))
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;

                        pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                        //pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        //pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                        pipeline.AddLast("mqtt-encoder", new MqttEncoder());
                        pipeline.AddLast("mqtt-decoder", new MqttDecoder(true, 256 * 1024));
                        pipeline.AddLast("string-encoder", new StringEncoder());
                        pipeline.AddLast("string-decoder", new StringDecoder());
                        pipeline.AddLast("echo", new EchoServerHandler());
                    }));

                var boundChannel = await bootstrap.BindAsync(9998);

                Console.ReadLine();

                await boundChannel.CloseAsync();
            }
            finally
            {
                await Task.WhenAll(
                    bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(1)),
                    workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(1))
                    );
            }
        }
    }
}