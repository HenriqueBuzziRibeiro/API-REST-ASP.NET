using System.ComponentModel.DataAnnotations;

namespace Usuarios.API.DTOs;

// O que o cliente PODE enviar ao atualizar um usuário.
// Ativo entra aqui (é como se reativa alguém), mas Id não:
// ele vem na URL (/api/usuarios/5), não no corpo.
public class AtualizarUsuarioDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
    public string Email { get; set; } = string.Empty;

    [Range(0, 130, ErrorMessage = "A idade deve estar entre 0 e 130.")]
    public int Idade { get; set; }

    [Required(ErrorMessage = "O cargo é obrigatório.")]
    [StringLength(60, ErrorMessage = "O cargo deve ter no máximo 60 caracteres.")]
    public string Cargo { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;
}