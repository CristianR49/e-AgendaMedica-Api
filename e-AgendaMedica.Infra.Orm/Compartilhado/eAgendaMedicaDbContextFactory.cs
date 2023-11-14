using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace e_AgendaMedica.Infra.Orm.Compartilhado
{
    public class eAgendaMedicaDbContextFactory : IDesignTimeDbContextFactory<eAgendaMedicaDbContext>
    {
        public eAgendaMedicaDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<eAgendaMedicaDbContext> optionsBuilder = new DbContextOptionsBuilder<eAgendaMedicaDbContext>();

            optionsBuilder.UseSqlServer(@"Data Source=(LOCALDB)\MSSQLLOCALDB;Initial Catalog=eAgendaMedica;Integrated Security=True");

            eAgendaMedicaDbContext dbContext = new eAgendaMedicaDbContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}
