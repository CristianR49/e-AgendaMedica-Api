using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using e_AgendaMedica.Infra.Orm.Compartilhado;
using e_AgendaMedica.Infra.Orm.ModuloAtividade;
using e_AgendaMedica.Infra.Orm.ModuloMedico;
using FizzWare.NBuilder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eAgendaMedica.TestesIntegracao.Compartilhado
{
    public class TestesIntegracaoBase
    {
        protected IRepositorioMedico RepositorioMedico { get; set; }
        protected IRepositorioAtividade RepositorioAtividade { get; set; }
        protected IContextoPersistencia ContextoPersistencia { get; set; }


        public TestesIntegracaoBase()
        {
            LimparTabelas();

            string? connectionString = ObterConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<eAgendaMedicaDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            var dbContext = new eAgendaMedicaDbContext(optionsBuilder.Options);
            ContextoPersistencia = dbContext;

            RepositorioAtividade = new RepositorioAtividadeOrm(dbContext);
            RepositorioMedico = new RepositorioMedicoOrm(dbContext);
            
                                        

            BuilderSetup.SetCreatePersistenceMethod<Medico>(medico =>
            {
                RepositorioMedico.Inserir(medico);
                ContextoPersistencia.Gravar();
            });


            BuilderSetup.SetCreatePersistenceMethod<Atividade>(avitidade =>
            {
                RepositorioAtividade.Inserir(avitidade);
                ContextoPersistencia.Gravar();
            });
        }

        protected static void LimparTabelas()
        {
            string? connectionString = ObterConnectionString();

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            string sqlLimpezaTabela =
                 "DELETE FROM [DBO].[TBMedico];"
               + "DELETE FROM [DBO].[TBAtividade];"
               + "DELETE FROM [DBO].[TBAtividade_TBMedico];";


            SqlCommand comando = new SqlCommand(sqlLimpezaTabela, sqlConnection);

            sqlConnection.Open();

            comando.ExecuteNonQuery();

            sqlConnection.Close();
        }

        protected static string? ObterConnectionString()
        {
            var configuracao = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");
            return connectionString;
        }
    }
}

