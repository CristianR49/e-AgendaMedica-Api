using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using e_AgendaMedica.Infra.Orm.ModuloAtividade;
using e_AgendaMedica.Infra.Orm.ModuloMedico;
using Microsoft.EntityFrameworkCore;

namespace e_AgendaMedica.Infra.Orm.Compartilhado
{
    public class eAgendaMedicaDbContext : DbContext, IContextoPersistencia
    {
        public eAgendaMedicaDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MapeadorMedicoOrm());

            modelBuilder.ApplyConfiguration(new MapeadorAtividadeOrm());

        }

        public async Task<bool> GravarAsync()
        {
            await SaveChangesAsync();
            return true;
        }
    }
}
