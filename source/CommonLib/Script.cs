namespace CommonLib;

public struct Script
{
	public const string UPDATE = @"UPDATE dbo.Clientes SET
									Nome				= @Nome,
									DataNascimento		= @DataNascimento,
									DataAtualizacao		= GETDATE()
									WHERE CpfCnpj		= @CpfCnpj";

	public const string INSERT = @"INSERT INTO dbo.Clientes (Id
														   ,CpfCnpj
														   ,Nome
														   ,DataNascimento
														   ,Ativo
														   ,DataInclusao)
													VALUES (@Id
														   ,@CpfCnpj
														   ,@Nome
														   ,@DataNascimento
														   ,1
														   ,GETDATE())";

	public const string ObterCliente = @"SELECT cast(Id as varchar(255)) as Id
										,Nome
										,DataNascimento
										,Ativo
							FROM dbo.Clientes(NOLOCK) WHERE Id = @ID AND Ativo = 1";
}
