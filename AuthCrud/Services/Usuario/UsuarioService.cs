using AuthCrud.Data;
using AuthCrud.Dto.Usuario;
using AuthCrud.Models;
using AuthCrud.Services.Senha;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuthCrud.Services.Usuario
{
    public class UsuarioService : IUsuarioInterface
    {
        #region ctor
        private readonly AppDbContext _dbContext;
        private readonly ISenhaInterface _senhaInterface;
        private readonly IMapper _mapper;
        public UsuarioService(AppDbContext dbContext, ISenhaInterface senhaInterface, IMapper mapper)
        {
            _dbContext = dbContext;
            _senhaInterface = senhaInterface;
            _mapper = mapper;
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

        public async Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDto usuarioCriacaoDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                if (!VerificaSeExisteEmailUsuarioRepetidos(usuarioCriacaoDto))
                {
                    response.Mensagem = "Email/User already registered!";
                    return response;
                }

                _senhaInterface.CriarSenhaHash(usuarioCriacaoDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                UsuarioModel usuario = _mapper.Map<UsuarioModel>(usuarioCriacaoDto);
                usuario.SenhaHash = senhaHash;
                usuario.SenhaSalt = senhaSalt;

                _dbContext.Add(usuario);
                await _dbContext.SaveChangesAsync();

                response.Mensagem = "Usuário cadastrado com sucesso!";
                response.Dados = usuario;
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
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
    
    private bool VerificaSeExisteEmailUsuarioRepetidos(UsuarioCriacaoDto usuarioCriacaoDto)
        {
            var usuario = _dbContext.Usuarios.FirstOrDefault(item => item.Email ==
            usuarioCriacaoDto.Email
            || item.User == usuarioCriacaoDto.User);
            return usuario != null ? false : true;
        }
    }
}
