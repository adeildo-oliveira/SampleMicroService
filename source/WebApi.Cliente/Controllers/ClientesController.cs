using CommonLib;
using CommonLib.Models;
using Confluent.Kafka;
using GrpcService.Cliente;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebApi.Cliente.Controllers;
[Route("api/[controller]")]
[ApiVersion("1", Deprecated = false)]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly IClienteGrpc _clienteGrpc;
    private readonly IKafkaConfig _kafkaConfig;
    private readonly Message<string, ClienteModel> _kafkaMessage;

    public ClientesController(IClienteGrpc clienteGrpc, IKafkaConfig kafkaConfig)
    {
        _clienteGrpc = clienteGrpc;
        _kafkaConfig = kafkaConfig;
        _kafkaMessage = new Message<string, ClienteModel>();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid id)
    {
        var cliente = await _clienteGrpc.Cliente.ObterClienteAsync(new RequestCliente
        {
            IdCliente = id.ToString()
        });

        switch (cliente.StatusCode)
        {
            case 204: return NoContent();
            case 404: return NotFound();
            default: return Ok(new ClienteModel
            {
                Id = Guid.Parse(cliente.Id),
                Nome = cliente.Nome,
                DataNascimento = cliente.DataNascimento.ToDateTime(),
                Ativo = (CommonLib.Models.TipoStatus)cliente.Ativo
            });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] ClienteModel clienteModel)
    {
        using (var producer = new ProducerBuilder<string, ClienteModel>(_kafkaConfig.ProducerConfiguration())
            .SetValueSerializer(new CustomSerializer<ClienteModel>())
            .Build())
        {
            _kafkaMessage.Key = clienteModel.Id.ToString();
            _kafkaMessage.Value = clienteModel;

            var result = await producer.ProduceAsync(_kafkaConfig.Topic, _kafkaMessage);

            return StatusCode((int)HttpStatusCode.Accepted);
        }
    }
}
