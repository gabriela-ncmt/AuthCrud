using AuthCrud.Data;
using AuthCrud.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthCrud.Services.Auditoria
{
    public class AuditoriaService : IAuditoriaInterface
    {
        private readonly AppDbContext _context;

        public AuditoriaService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<AuditoriaModel>> BuscarAuditorias()
        {
            return await _context.Auditorias.OrderByDescending(a => a.Data).ToListAsync();

        }

        public async Task RegistrarAuditoriaAsync(string acao, string usuarioId, string dadosAlterados)
        {
            var auditoria = new AuditoriaModel
            {
                Acao = acao,
                UsuarioId = usuarioId,
                DadosAlterados = dadosAlterados
            };

            _context.Auditorias.Add(auditoria);
            await _context.SaveChangesAsync();
        }
    }
}
