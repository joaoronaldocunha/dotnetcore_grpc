using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using static GrpcGreeter.GrpcGreeters;

namespace GrpcGreeterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);  
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");

            var client = new GrpcGreetersClient(channel);
    
            var reply = await client.SayHelloAsync(
                              new GrpcGreeter.HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
            
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            Console.WriteLine("Starting Greeter Stream");
            using var replies = client.GetGreeterStream(new Empty(), cancellationToken: cts.Token);

            try
            {
                await foreach (var greeterData in replies.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
                {
                    Console.WriteLine($"{greeterData.DateTimeStamp.ToDateTime():s} | {greeterData.Message}");
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {               
                Console.WriteLine("Stream cancelled.");
            }
            
            Console.WriteLine("Press a key to exit");
            Console.ReadKey();
        }
    }
}