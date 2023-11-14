using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using Microsoft.EntityFrameworkCore;

namespace e_AgendaMedica.Infra.Orm.Compartilhado
{
    public class eAgendaMedicaDbContext : DbContext
    {
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Atividade> Atividades { get; set; }
        public eAgendaMedicaDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medico>(model =>
            {
                model.ToTable("TBMedico");

                model.Property(x => x.Id).ValueGeneratedNever();

                model.Property(x => x.Crm).IsRequired();
            });

            modelBuilder.Entity<Atividade>(model =>
            {
                model.ToTable("TBAtividade");

                model.Property(x => x.Id).ValueGeneratedNever();

                model.Property(x => x.Data).IsRequired();

                model.Property(x => x.HorarioInicio).IsRequired();

                model.Property(x => x.HorarioTermino).IsRequired();

                model.Property(x => x.TipoAtividade)
                .HasConversion<int>()
                .IsRequired();

                model.HasMany(x => x.Medicos)
                .WithMany()
                .UsingEntity(x => x.ToTable("TBAtividade_TBMedico"));


            });

            base.OnModelCreating(modelBuilder);

        }
    }
}
