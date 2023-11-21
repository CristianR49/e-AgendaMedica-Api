using e_AgendaMedica.Dominio.Compartilhado;

namespace e_AgendaMedica.Dominio.ModuloMedico
{
    public class Medico : Entidade
    {
        public string Nome { get; set; }
        public string Crm { get; set; }
        public Medico(string nome, string crm)
        {
            Nome = nome;
            Crm = crm;
         }
        public Medico()
        {

        }
    }
}
