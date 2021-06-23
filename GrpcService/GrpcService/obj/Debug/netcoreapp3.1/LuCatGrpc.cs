// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/LuCat.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace GrpcService {
  /// <summary>
  ///定义服务
  /// </summary>
  public static partial class LuCat
  {
    static readonly string __ServiceName = "LuCat.LuCat";

    static readonly grpc::Marshaller<global::GrpcService.SuckingCatRequest> __Marshaller_LuCat_SuckingCatRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GrpcService.SuckingCatRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::GrpcService.SuckingCatResult> __Marshaller_LuCat_SuckingCatResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GrpcService.SuckingCatResult.Parser.ParseFrom);

    static readonly grpc::Method<global::GrpcService.SuckingCatRequest, global::GrpcService.SuckingCatResult> __Method_SuckingCat = new grpc::Method<global::GrpcService.SuckingCatRequest, global::GrpcService.SuckingCatResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SuckingCat",
        __Marshaller_LuCat_SuckingCatRequest,
        __Marshaller_LuCat_SuckingCatResult);

    static readonly grpc::Method<global::GrpcService.SuckingCatRequest, global::GrpcService.SuckingCatResult> __Method_SuckingdCat = new grpc::Method<global::GrpcService.SuckingCatRequest, global::GrpcService.SuckingCatResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SuckingdCat",
        __Marshaller_LuCat_SuckingCatRequest,
        __Marshaller_LuCat_SuckingCatResult);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::GrpcService.LuCatReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of LuCat</summary>
    [grpc::BindServiceMethod(typeof(LuCat), "BindService")]
    public abstract partial class LuCatBase
    {
      /// <summary>
      ///定义吸猫方法
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::GrpcService.SuckingCatResult> SuckingCat(global::GrpcService.SuckingCatRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::GrpcService.SuckingCatResult> SuckingdCat(global::GrpcService.SuckingCatRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(LuCatBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SuckingCat, serviceImpl.SuckingCat)
          .AddMethod(__Method_SuckingdCat, serviceImpl.SuckingdCat).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, LuCatBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_SuckingCat, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GrpcService.SuckingCatRequest, global::GrpcService.SuckingCatResult>(serviceImpl.SuckingCat));
      serviceBinder.AddMethod(__Method_SuckingdCat, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GrpcService.SuckingCatRequest, global::GrpcService.SuckingCatResult>(serviceImpl.SuckingdCat));
    }

  }
}
#endregion
