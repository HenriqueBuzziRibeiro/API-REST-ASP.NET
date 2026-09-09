using System.ComponentModel.DataAnnotations;

namespace Usuarios.API.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")] [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O email é obrigatório.")] [EmailAddress] [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        [Range(0, 130)]
        public int Idade { get; set; }
        [Required(ErrorMessage = "O cargo é obrigatório.")] [StringLength(60)]
        public string Cargo { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; }
    }
}
