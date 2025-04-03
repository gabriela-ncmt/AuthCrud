using AuthCrud.Data;
using AuthCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthCrud.Services.Usuario
{
    public class UsuarioService : IUsuarioInterface
    {
        #region ctor
        private readonly AppDbContext _dbContext;
        public UsuarioService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion
        public async Task<ResponseModel<List<UsuarioModel>>> ListarUsuarios()
        {
            ResponseModel<List<UsuarioModel>> response= new ResponseModel<List<UsuarioModel>>();
            try
            {
                var usuarios = await _dbContext.Usuarios.ToListAsync();
                if (!usuarios.Any())
                {
                    response.Mensagem = "Nenhum usuário cadastrado!";
                    return response;
                }
                response.Dados = usuarios;
                response.Mensagem = "Usuarios localizados com sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> ObterUsuarioPorId(int id)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = await _dbContext.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    response.Mensagem = "User not found!";
                    return response;
                }

                response.Dados = usuario;
                response.Mensagem = "User found!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem= ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> RemoverUsuario(int id)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = await _dbContext.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    response.Mensagem = "User not found!";
                    response.Status = false;
                    return response;
                }
                _dbContext.Usuarios.Remove(usuario);
                await _dbContext.SaveChangesAsync();

                response.Mensagem = $"Deleted user: {usuario.Nome} successfully!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem+= ex.Message;
                response.Status = false;
                return response ;
            }
        }
    }
}
