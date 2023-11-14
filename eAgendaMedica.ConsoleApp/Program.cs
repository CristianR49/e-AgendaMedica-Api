using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using e_AgendaMedica.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eAgendaMedica.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Medico medico = new Medico("aaaa");

            DbContextOptionsBuilder<eAgendaMedicaDbContext> optionsBuilder = new DbContextOptionsBuilder<eAgendaMedicaDbContext>();

            IConfiguration configuracao = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");

            optionsBuilder.UseSqlServer(connectionString);
            
            eAgendaMedicaDbContext dbContext = new eAgendaMedicaDbContext(optionsBuilder.Options);

            List<Medico> medicos = new List<Medico>();

            medicos.Add(medico);

            Atividade atividade = new Atividade(new DateTime(1999, 5, 20), new TimeSpan(20, 0, 0), new TimeSpan(22, 0, 0), TipoAtividadeEnum.Cirurgia, medicos);

            dbContext.Atividades.Add(atividade);

            dbContext.SaveChanges();
        }
    }
}