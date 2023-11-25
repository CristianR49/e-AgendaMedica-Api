using e_AgendaMedica.Dominio.Compartilhado;

namespace e_AgendaMedica.Dominio.ModuloMedico
{
    public class Medico : Entidade<Medico>
    {
        public string Nome { get; set; }
        public string Crm { get; set; }
        public Medico(string nome, string crm)
        {
            Nome = nome;
            Crm = crm;
        }

        public Medico(Guid id, string nome, string crm) : this(nome,crm)
        {
            Id = id;
        }

        public Medico()
        {

        }
    }
}
