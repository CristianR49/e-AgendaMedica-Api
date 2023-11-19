using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using e_AgendaMedica.Infra.Orm.Compartilhado;
using e_AgendaMedica.Infra.Orm.ModuloAtividade;
using e_AgendaMedica.Infra.Orm.ModuloMedico;
using eAgendaMedica.Aplicacao.ModuloAtividade;
using eAgendaMedica.Aplicacao.ModuloMedico;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eAgendaMedica.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Medico medico = new Medico("aaaa2");

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

            Atividade atividade = new Atividade(new DateTime(1555, 5, 20), new TimeSpan(20, 0, 0), new TimeSpan(22, 0, 0), TipoAtividadeEnum.Cirurgia, medicos);

            dbContext.Add(medico);

            //var repositorioAtividade = new RepositorioAtividadeOrm(dbContext);

            //ServicoAtividade servicoAtividade = new ServicoAtividade(repositorioAtividade, dbContext);

            

            //servicoMedico.InserirAsync(medico);

            

            //servicoAtividade.InserirAsync(atividade);

            dbContext.SaveChanges();
        }
    }
}