using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Core.Client.Models;
using Service.Grpc;
using Service.TokenRate.Client;
using Service.TokenRate.Grpc;
using Service.TokenRate.Grpc.Models;
using GrpcClientFactory = ProtoBuf.Grpc.Client.GrpcClientFactory;

namespace TestApp
{
	public class Program
	{
		private static async Task Main()
		{
			GrpcClientFactory.AllowUnencryptedHttp2 = true;
			ILogger<Program> logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Program>();

			Console.Write("Press enter to start");
			Console.ReadLine();

			var factory = new TokenRateClientFactory("http://localhost:5001", logger);
			IGrpcServiceProxy<ITokenRateService> serviceProxy = factory.GetTokenRateService();
			ITokenRateService client = serviceProxy.Service;

			CommonGrpcResponse setResponse1 = await client.SetTokenRate(new SetTokenRateGrpcRequest {Value = 10m});
			if (setResponse1?.IsSuccess == false)
				throw new Exception("Can't set (1) value for rate");

			TokenRateGrpcResponse request1 = await client.GetTokenRate();
			if (request1 is not {Value: 10m })
				throw new Exception("Can't get (1) value for rate");

			Console.Write("Value 1: ");
			Console.WriteLine(JsonConvert.SerializeObject(request1));

			CommonGrpcResponse setResponse2 = await client.SetTokenRate(new SetTokenRateGrpcRequest {Value = 100.5m});
			if (setResponse2?.IsSuccess == false)
				throw new Exception("Can't set (2) value for rate");

			TokenRateGrpcResponse request2 = await client.GetTokenRate();
			if (request2 is not { Value: 100.5m })
				throw new Exception("Can't get (2) value for rate");

			Console.Write("Value 2: ");
			Console.WriteLine(JsonConvert.SerializeObject(request2));

			Console.WriteLine("End");
			Console.ReadLine();
		}
	}
}