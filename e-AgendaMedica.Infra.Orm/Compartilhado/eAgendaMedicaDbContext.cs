using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using e_AgendaMedica.Infra.Orm.ModuloAtividade;
using e_AgendaMedica.Infra.Orm.ModuloMedico;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace e_AgendaMedica.Infra.Orm.Compartilhado
{
    public class eAgendaMedicaDbContext : DbContext
    {
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Atividade> Atividades { get; set; }
        public eAgendaMedicaDbContext(DbContextOptions options) : base(options)
        {
        }

        public eAgendaMedicaDbContext CreateDbContext(string [] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<eAgendaMedicaDbContext>();

            IConfiguration configuracao = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");

            optionsBuilder.UseSqlServer(connectionString);

            var dbContext = new eAgendaMedicaDbContext(optionsBuilder.Options);

            return dbContext;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MapeadorMedicoOrm());

            modelBuilder.ApplyConfiguration(new MapeadorAtividadeOrm());

        }
    }
}
