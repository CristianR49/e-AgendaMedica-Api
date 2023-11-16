namespace e_AgendaMedica.Dominio.Compartilhado
{
    internal interface IContextoPersistencia
    {
        Task<bool> GravarAsync();
    }
}
