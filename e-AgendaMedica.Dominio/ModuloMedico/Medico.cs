using e_AgendaMedica.Dominio.Compartilhado;

namespace e_AgendaMedica.Dominio.ModuloMedico
{
    public class Medico : EntidadeBase
    {
        public string Crm { get; set; }
        public Medico(string crm)
        {
            Crm = crm;
        }
    }
}
