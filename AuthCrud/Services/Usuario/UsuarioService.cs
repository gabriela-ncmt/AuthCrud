using AuthCrud.Data;
using AuthCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthCrud.Services.Usuario
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly AppDbContext _dbContext;
        public UsuarioService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
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
    }
}
