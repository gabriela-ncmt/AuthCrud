using AuthCrud.Models;

namespace AuthCrud.Services.Usuario
{
    public interface IUsuarioInterface
    {
        Task<ResponseModel<List<UsuarioModel>>> ListarUsuarios();
    }
}
