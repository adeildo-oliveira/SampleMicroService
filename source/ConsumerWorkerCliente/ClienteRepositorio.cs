using CommonLib;
using CommonLib.Models;
using Dapper;
using System.Data;

namespace WorkerConsumerServiceCliente;

public interface IClienteRepositorio
{
	ValueTask<bool> AtualizarOuInserirAsync(ClienteModel request);
}

public class ClienteRepositorio : IClienteRepositorio
{
	private readonly IContextDapper _contextDapper;

    public ClienteRepositorio(IContextDapper contextDapper) => _contextDapper = contextDapper;

    public async ValueTask<bool> AtualizarOuInserirAsync(ClienteModel request)
	{
		const string sql = $@"{Script.UPDATE}
							IF @@ROWCOUNT = 0
							BEGIN
								{Script.INSERT}
							END";

		using var conn = _contextDapper.Connection();
		await conn.ExecuteAsync(sql, param: new 
		{ 
			request.Id,
			request.CpfCnpj,
			request.Nome,
			request.DataNascimento
		}, commandType: CommandType.Text);

		return true;
	}
}