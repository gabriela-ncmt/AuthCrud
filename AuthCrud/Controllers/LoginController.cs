using AuthCrud.Dto.Login;
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuario = await _usuarioInterface.Login(usuarioLoginDto);
            return Ok(usuario);
        }

    }
}
