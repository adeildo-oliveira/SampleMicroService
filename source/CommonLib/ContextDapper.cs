using System.Data;
using System.Data.SqlClient;

namespace CommonLib;

public interface IContextDapper
{
    IDbConnection Connection();
}

public class ContextDapper : IContextDapper
{
    private readonly string _connectionString;

    public ContextDapper(string connectionString) => _connectionString = connectionString;

    public IDbConnection Connection() => new SqlConnection(_connectionString);
}