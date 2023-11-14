using e_AgendaMedica.Dominio.ModuloMedico;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_AgendaMedica.Infra.Orm.ModuloMedico
{
    public class MapeadorMedicoOrm : IEntityTypeConfiguration<Medico>
    {
        public void Configure(EntityTypeBuilder<Medico> builder)
        {


            builder.ToTable("TBMedico");

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Crm).IsRequired();

        }
    }
}