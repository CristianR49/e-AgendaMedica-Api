namespace e_AgendaMedica.Dominio.Compartilhado
{
    public interface IContextoPersistencia
    {
        Task<bool> GravarAsync();
        bool Gravar();
    }
}
