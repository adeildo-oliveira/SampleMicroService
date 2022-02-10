namespace CommonLib.Models;

public record ClienteModel
{
    public Guid Id { get; init; }
    public string? CpfCnpj { get; init; }
    public string? Nome { get; init; }
    public DateTime DataNascimento { get; init; }
    public TipoStatus Ativo { get; init; }
}