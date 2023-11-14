using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using e_AgendaMedica.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace eAgendaMedica.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Medico medico = new Medico("aaaa");

            DbContextOptionsBuilder<eAgendaMedicaDbContext> optionsBuilder = new DbContextOptionsBuilder<eAgendaMedicaDbContext>();

            optionsBuilder.UseSqlServer(@"Data Source=(LOCALDB)\MSSQLLOCALDB;Initial Catalog=eAgendaMedica;Integrated Security=True");

            eAgendaMedicaDbContext dbContext = new eAgendaMedicaDbContext(optionsBuilder.Options);

            List<Medico> medicos = new List<Medico>();

            medicos.Add(medico);

            Atividade atividade = new Atividade(new DateTime(1999, 5, 20), new TimeSpan(20, 0, 0), new TimeSpan(22, 0, 0), TipoAtividadeEnum.Cirurgia, medicos);

            dbContext.Atividades.Add(atividade);

            dbContext.SaveChanges();
        }
    }
}