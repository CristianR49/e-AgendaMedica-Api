namespace e_AgendaMedica.Dominio.Compartilhado
{
    public class Entidade<T>
    {
        public Guid Id { get; set; }
        public Entidade()
        {
            Id = Guid.NewGuid();
        }
    }
}
