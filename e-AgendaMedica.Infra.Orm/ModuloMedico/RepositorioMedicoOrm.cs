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

        public Medico? SelecionarPorNome(string nome)
        {
            return registros.FirstOrDefault(x => x.Nome == nome);
        }

        public Medico? SelecionarPorCrm(string crm)
        {
            return registros.FirstOrDefault(x => x.Crm == crm);
        }

        public List<Medico> SelecionarMuitos(List<Guid> idsMedicosSelecionados)
        {
            return registros.Where(medico => idsMedicosSelecionados.Contains(medico.Id)).ToList();
        }
    }
}
