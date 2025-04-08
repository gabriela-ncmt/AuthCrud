using AuthCrud.Dto.Usuario;
using AuthCrud.Services.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace AuthCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioInterface _usuarioInterface;
        public LoginController(IUsuarioInterface usuarioInterface)
        {
            _usuarioInterface = usuarioInterface;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegistrarUsuario(UsuarioCriacaoDto usuarioCriacao)
        {
            var usuario = await _usuarioInterface.RegistrarUsuario(usuarioCriacao);
            return Ok(usuario);
        }

    }
}
