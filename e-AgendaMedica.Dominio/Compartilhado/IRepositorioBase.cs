using e_AgendaMedica.Dominio.ModuloAtividade;

namespace e_AgendaMedica.Dominio.Compartilhado
{
    public interface IRepositorioBase<TEntidade> where TEntidade : Entidade<TEntidade>
    {
        void Editar(TEntidade registro);
        void Excluir(TEntidade registro);
        Task<bool> InserirAsync(TEntidade registro);
        bool Inserir(TEntidade registro);
        Task<TEntidade> SelecionarPorIdAsync(Guid id);
        TEntidade SelecionarPorId(Guid id);
        Task<List<TEntidade>> SelecionarTodosAsync();
        List<TEntidade> SelecionarTodos();
        bool Existe(TEntidade registro);
    }
}
