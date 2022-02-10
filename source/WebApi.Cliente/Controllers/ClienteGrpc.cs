using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using WebApi.Cliente.Models;
using GrpcCliente = GrpcService.Cliente.Cliente.ClienteClient;

namespace WebApi.Cliente.Controllers;

public interface IClienteGrpc
{
    GrpcCliente Cliente { get; }
}

public class ClienteGrpc : IClienteGrpc
{
    private readonly GrpcOption _grpcOption;

    public ClienteGrpc(IOptions<GrpcOption> options) => _grpcOption = options.Value;

    private GrpcCliente? _clientCliente = null;

    public GrpcCliente Cliente
    {
        get
        {
            if (_clientCliente == null)
            {
                var channel = GrpcChannel.ForAddress(_grpcOption.ServiceUrlCliente, GrpcOption());
                _clientCliente = new GrpcCliente(channel);
            }

            return _clientCliente;
        }
    }

    private GrpcChannelOptions GrpcOption() => new()
    {
        HttpHandler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            KeepAlivePingDelay = TimeSpan.FromSeconds(_grpcOption.KeepAlivePingDelay),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(_grpcOption.KeepAlivePingTimeout),
            EnableMultipleHttp2Connections = _grpcOption.EnableMultipleHttp2Connections
        }
    };
}
