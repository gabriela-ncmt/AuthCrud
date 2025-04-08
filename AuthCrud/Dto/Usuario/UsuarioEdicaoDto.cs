using System.ComponentModel.DataAnnotations;

namespace AuthCrud.Dto.Usuario
{
    public class UsuarioEdicaoDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "User is required")]
        public string User { get; set; }
        [Required(ErrorMessage = "Nome is required")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Sobrenome is required")]
        public string Sobrenome { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        [Required(ErrorMessage = "Senha is required")]
        public byte[] SenhaHash { get; set; }
        public byte[] SenhaSalt { get; set; }
    }
}
