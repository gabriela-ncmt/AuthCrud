using AuthCrud.Data;
using AuthCrud.Dto.Login;
using AuthCrud.Dto.Usuario;
using AuthCrud.Models;
using AuthCrud.Services.Auditoria;
using AuthCrud.Services.Senha;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

namespace AuthCrud.Services.Usuario
{
    public class UsuarioService : IUsuarioInterface
    {
        #region ctor
        private readonly AppDbContext _dbContext;
        private readonly ISenhaInterface _senhaInterface;
        private readonly IMapper _mapper;
        private readonly IAuditoriaInterface _auditoriaInterface;
        private readonly IHttpContextAccessor _contextAccessor;
        public UsuarioService(
            AppDbContext dbContext,
            ISenhaInterface senhaInterface, 
            IMapper mapper,
            IAuditoriaInterface auditoriaInterface,
            IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _senhaInterface = senhaInterface;
            _mapper = mapper;
            _auditoriaInterface = auditoriaInterface;
            _contextAccessor = contextAccessor;
        }
        #endregion
        public async Task<ResponseModel<UsuarioModel>> EditarUsuario(UsuarioEdicaoDto usuarioEdicaoDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                UsuarioModel usuarioBanco = await _dbContext.Usuarios.FindAsync(usuarioEdicaoDto.Id);
                if (usuarioBanco == null)
                {
                    response.Mensagem = "Usuário não localizado!";
                    return response;
                }

                var dadosAntes = JsonConvert.SerializeObject(usuarioBanco);

                usuarioBanco.Nome = usuarioEdicaoDto.Nome;
                usuarioBanco.Sobrenome = usuarioEdicaoDto.Sobrenome;
                usuarioBanco.Email = usuarioEdicaoDto.Email;
                usuarioBanco.User = usuarioEdicaoDto.User;
                usuarioBanco.DataAlteracao = DateTime.Now;

                _dbContext.Update(usuarioBanco);
                await _dbContext.SaveChangesAsync();

                response.Mensagem = "Usuário editado com sucesso!";
                response.Dados = usuarioBanco;

                var dadosDepois = JsonConvert.SerializeObject(usuarioBanco);

                var usuarioId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                await _auditoriaInterface.RegistrarAuditoriaAsync(
                    "Atualização", 
                    usuarioId,
                    $"Antes: {dadosAntes}, Depois: {dadosDepois}");

                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
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

        public async Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = await _dbContext.Usuarios.FirstOrDefaultAsync(userBanco =>
                userBanco.Email == usuarioLoginDto.Email);

                if (usuario == null)
                {
                    response.Mensagem = "Credenciais inválidas!";
                    return response;
                }

                if (!_senhaInterface.VerificaSenhaHash(usuarioLoginDto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    response.Mensagem = "Credenciais inválidas!";
                    return response;
                }

                var token = _senhaInterface.CriarToken(usuario);

                usuario.Token = token;
                _dbContext.Update(usuario);
                await _dbContext.SaveChangesAsync();

                response.Dados = usuario;
                response.Mensagem = "Usuário logado com sucesso!";

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
                usuario.DataCriacao = DateTime.Now;
                usuario.DataAlteracao = DateTime.Now;

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
                var dadosAntes = JsonConvert.SerializeObject(usuario);

                _dbContext.Usuarios.Remove(usuario);
                await _dbContext.SaveChangesAsync();

                response.Mensagem = $"Deleted user: {usuario.Nome} successfully!";

                var usuarioId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

                await _auditoriaInterface.RegistrarAuditoriaAsync(
                    "Remoção",
                    usuarioId,
                    $"Antes: {dadosAntes}");

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
