namespace e_AgendaMedica.Dominio.Compartilhado
{
    public class EntidadeBase
    {
        public Guid Id { get; set; }
        public EntidadeBase()
        {
            Id = Guid.NewGuid();
        }
    }
}
