using CommonLib;
using Dapper;
using System.Data;
using GoogleTimeStamp = Google.Protobuf.WellKnownTypes.Timestamp;

namespace GrpcService.Cliente.Repositorio;

public interface IClienteRepositorio
{
    Task<Response> ObterClienteAsync(RequestCliente request);
}

public class ClienteRepositorio : IClienteRepositorio
{
    private readonly IContextDapper _contextDapper;

    public ClienteRepositorio(IContextDapper contextDapper)
    {
        _contextDapper = contextDapper;
        SqlMapper.ResetTypeHandlers();
        SqlMapper.AddTypeHandler(new ProtobufTimestampHandler());
    }

    public async Task<Response> ObterClienteAsync(RequestCliente request)
    {
        using var conn = _contextDapper.Connection();
        return await conn.QueryFirstOrDefaultAsync<Response>(Script.ObterCliente, param: new { ID = request.IdCliente }, commandType: CommandType.Text);
    }
}

public class ProtobufTimestampHandler : SqlMapper.TypeHandler<GoogleTimeStamp>
{
    public override void SetValue(IDbDataParameter parameter, GoogleTimeStamp value) => parameter.Value = value;

    public override GoogleTimeStamp Parse(object value) => GoogleTimeStamp.FromDateTime(DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc));
}
