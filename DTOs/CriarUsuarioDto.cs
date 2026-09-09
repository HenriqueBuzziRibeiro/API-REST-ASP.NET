using System.ComponentModel.DataAnnotations;

namespace Usuarios.API.DTOs;
// O que o cliente PODE enviar ao criar um usuário.
// Id, Ativo e DataCadastro estão ausentes de propósito: são do servidor.
//Caso no Json tenha um campo que não existe no DTO ele vai ser descartado, não vai dar erro. Mas se faltar algum campo obrigatório, vai dar erro.
public class CriarUsuarioDto
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
    }