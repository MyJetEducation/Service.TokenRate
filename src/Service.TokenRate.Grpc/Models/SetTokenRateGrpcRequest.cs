using System.Runtime.Serialization;

namespace Service.TokenRate.Grpc.Models
{
	[DataContract]
	public class SetTokenRateGrpcRequest
	{
		[DataMember(Order = 1)]
		public decimal Value { get; set; }
	}
}