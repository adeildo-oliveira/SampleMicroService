using Grpc.Core;
using GrpcService.Cliente.Repositorio;

namespace GrpcService.Cliente.Services;

public class ClienteService : Cliente.ClienteBase
{
    private readonly ILogger<ClienteService> _logger;
    private readonly IClienteRepositorio _clienteRepository;

    public ClienteService(ILogger<ClienteService> logger, IClienteRepositorio clienteRepository)
    {
        _logger = logger;
        _clienteRepository = clienteRepository;
    }

    public override async Task<Response> ObterCliente(RequestCliente request, ServerCallContext context)
    {
        Response result = new();

        try
        {
            _logger.LogInformation("MENSAGEM RECEBIDA {time} {idcliente}", DateTimeOffset.Now, request.IdCliente);
            _logger.LogInformation("CONSULTANDO CLIENTE NA BASE DE DADOS");
            result = await _clienteRepository.ObterClienteAsync(request);

            if (result is null)
            {
                _logger.LogInformation("CLIENTE NAO ENCONTRADO {IdCliente}", request.IdCliente);
                return new Response
                {
                    StatusCode = 404
                };
            }

            result.StatusCode = 200;
            _logger.LogInformation("CLIENTE ENCONTRADO {idcliente}", request.IdCliente);

            return result;
        }
        catch (Exception erro)
        {
            _logger.LogError(erro, erro.Message);
            
            result.StatusCode = 500;
            return result;
        }
    }
}
