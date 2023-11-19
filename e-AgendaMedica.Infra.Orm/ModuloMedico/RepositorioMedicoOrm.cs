using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloMedico;
using e_AgendaMedica.Infra.Orm.Compartilhado;

namespace e_AgendaMedica.Infra.Orm.ModuloMedico
{
    public class RepositorioMedicoOrm : RepositorioBase<Medico>, IRepositorioMedico
    {
        public RepositorioMedicoOrm(IContextoPersistencia dbContext) : base(dbContext)
        {
        }
    }
}
