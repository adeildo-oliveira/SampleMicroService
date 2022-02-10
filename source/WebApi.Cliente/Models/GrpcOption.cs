namespace WebApi.Cliente.Models;

public class GrpcOption
{
    public string? ServiceUrlCliente { get; set; }
    public int KeepAlivePingDelay { get; set; }
    public int KeepAlivePingTimeout { get; set; }
    public bool EnableMultipleHttp2Connections { get; set; }
}
