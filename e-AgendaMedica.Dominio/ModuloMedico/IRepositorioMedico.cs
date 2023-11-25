using e_AgendaMedica.Dominio.Compartilhado;

namespace e_AgendaMedica.Dominio.ModuloMedico
{
    public interface IRepositorioMedico : IRepositorioBase<Medico>
    {
        public Medico? SelecionarPorNome(string nome);
        public Medico? SelecionarPorCrm(string crm);
        public List<Medico> SelecionarMuitos(List<Guid> idsMedicosSelecionados);
    }
}
