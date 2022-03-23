using System.ServiceModel;
using System.Threading.Tasks;
using Service.Core.Client.Models;
using Service.TokenRate.Grpc.Models;

namespace Service.TokenRate.Grpc
{
	[ServiceContract]
	public interface ITokenRateService
	{
		[OperationContract]
		ValueTask<TokenRateGrpcResponse> GetTokenRate();

		[OperationContract]
		ValueTask<CommonGrpcResponse> SetTokenRate(SetTokenRateGrpcRequest request);
	}
}