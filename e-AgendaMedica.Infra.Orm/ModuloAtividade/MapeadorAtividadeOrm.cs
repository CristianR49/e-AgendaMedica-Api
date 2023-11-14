using e_AgendaMedica.Dominio.ModuloAtividade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace e_AgendaMedica.Infra.Orm.ModuloAtividade
{
    public class MapeadorAtividadeOrm : IEntityTypeConfiguration<Atividade>
    {
        public void Configure(EntityTypeBuilder<Atividade> builder)
        {

            builder.ToTable("TBAtividade");

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Data).IsRequired();

            builder.Property(x => x.HorarioInicio).IsRequired();

            builder.Property(x => x.HorarioTermino).IsRequired();

            builder.Property(x => x.TipoAtividade)
            .HasConversion<int>()
            .IsRequired();

            builder.HasMany(x => x.Medicos)
            .WithMany()
            .UsingEntity(x => x.ToTable("TBAtividade_TBMedico"));


        }
    }
}
