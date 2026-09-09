namespace Usuarios.API.DTOs;

// O que a API DEVOLVE. Só sai, nunca entra — por isso nenhuma validação:
// os dados vêm do próprio banco, já são confiáveis.
// Num sistema real, é aqui que você OMITIRIA senha, hash, CPF completo etc.
public class UsuarioRespostaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Idade { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; }
}