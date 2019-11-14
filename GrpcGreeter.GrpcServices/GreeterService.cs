using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using static GrpcGreeter.GrpcGreeters;

namespace GrpcGreeter.GrpcServices
{
    public class GreeterService : GrpcGreetersBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task GetGreeterStream(
            Empty _,
            IServerStreamWriter<GreeterData> responseStream,
            ServerCallContext context)
        {
            string[] welcomeMessages = {
                "Welcome",
                "Bienvenido",
                "Benvenuto",
                "Bienvenue",
                "Willkommen",
                "歓迎",
                "приветствовать"
            };

            var rng = new Random();
            
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000); // Gotta look busy
                
                var greeterData = new GreeterData
                {
                    DateTimeStamp = Timestamp.FromDateTime(DateTime.UtcNow),
                    Message = welcomeMessages[rng.Next(0, welcomeMessages.Length-1)]
                };

                _logger.LogInformation("Sending welcome message");

                await responseStream.WriteAsync(greeterData);        
            }
        }  
    }
}
