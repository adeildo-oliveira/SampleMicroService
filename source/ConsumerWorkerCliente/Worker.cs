using CommonLib;
using CommonLib.Models;
using Confluent.Kafka;
using System.Data.SqlClient;

namespace WorkerConsumerServiceCliente;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IKafkaConfig _kafkaConfig;
    private readonly IClienteRepositorio _clienteRepository;
    private ConsumeResult<string, ClienteModel> consumeResult;

    public Worker(ILogger<Worker> logger, IKafkaConfig kafkaConfig, IClienteRepositorio clienteRepository)
    {
        _logger = logger;
        _kafkaConfig = kafkaConfig;
        _clienteRepository = clienteRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("INICIANDO CONSUMER CLIENTE: {time}", DateTimeOffset.Now);

        var cts = new CancellationTokenSource();
        CancelarExecucao(cts);

        try
        {
            _logger.LogInformation("SUBSCREVENDO NO TOPICO DA MENSAGEM");
            using (var consumer = new ConsumerBuilder<string, ClienteModel>(_kafkaConfig.ConsumerConfiguration())
                .SetValueDeserializer(new CustomDeserializer<ClienteModel>())
                .Build())
            {
                await SendMessage(cts, consumer, stoppingToken);
            }
        }
        catch (Exception erro)
        {
            _logger.LogError(erro.Message, erro);
        }
    }

    private async Task SendMessage(CancellationTokenSource cts, IConsumer<string, ClienteModel>? consumer, CancellationToken stoppingToken)
    {
        _logger.LogInformation("SUBSCRIBE TOPIC {topic}", _kafkaConfig.Topic);
        consumer.Subscribe(_kafkaConfig.Topic);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                consumeResult = consumer?.Consume(cts.Token);

                _logger.LogInformation("INSERE/ATUALIZA CLIENTE");
                await _clienteRepository.AtualizarOuInserirAsync(consumeResult?.Message.Value);
                _logger.LogInformation("ATUALIZACAO/INCLUSAO REALIZADA COM SUCESSO");

                _logger.LogInformation("COMMIT KAFKA");
                consumer?.Commit(consumeResult);
                _logger.LogInformation("COMMIT REALIZADO COM SUCESSO");
            }
            catch (KafkaException erro)
            {
                await SendMessageDeadLetter(consumeResult, cts);
                _logger.LogError(erro.Message, erro);
            }
            catch (SqlException erro)
            {
                await SendMessageDeadLetter(consumeResult, cts);
                _logger.LogError(erro.Message, erro);
            }
        }
    }

    private async Task SendMessageDeadLetter(ConsumeResult<string, ClienteModel>? consumeResult, CancellationTokenSource cts)
    {
        using (var producer = new ProducerBuilder<string, ClienteModel>(_kafkaConfig.ProducerConfiguration())
            .SetValueSerializer(new CustomSerializer<ClienteModel>())
            .Build())
        {
            var result = await producer.ProduceAsync(_kafkaConfig.DeadLetter, consumeResult?.Message, cts.Token);

            _logger.LogInformation("{RESULT}", result.Status);
        }
    }

    private static void CancelarExecucao(CancellationTokenSource cts) => Console.CancelKeyPress += (_, e) =>
                                                                       {
                                                                           e.Cancel = true;
                                                                           cts.Cancel();
                                                                       };
}
