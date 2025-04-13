using System.ComponentModel.DataAnnotations;

namespace AuthCrud.Dto.Usuario
{
    public class UsuarioCriacaoDto
    {
        [Required(ErrorMessage = "User is required")]
        public string User { get; set; }
        [Required(ErrorMessage = "Nome is required")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Sobrenome is required")]
        public string Sobrenome { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
 
        [Required(ErrorMessage = "Senha is required")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "ConfirmaSenha is required"),
            Compare("Senha", ErrorMessage="Password must be equal")]
        public string ConfirmaSenha { get; set; }
    }
}
