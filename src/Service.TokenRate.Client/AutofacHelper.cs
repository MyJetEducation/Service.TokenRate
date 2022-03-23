using Autofac;
using Microsoft.Extensions.Logging;
using Service.TokenRate.Grpc;
using Service.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.TokenRate.Client
{
    public static class AutofacHelper
    {
        public static void RegisterTokenRateClient(this ContainerBuilder builder, string grpcServiceUrl, ILogger logger)
        {
            var factory = new TokenRateClientFactory(grpcServiceUrl, logger);

            builder.RegisterInstance(factory.GetTokenRateService()).As<IGrpcServiceProxy<ITokenRateService>>().SingleInstance();
        }
    }
}
