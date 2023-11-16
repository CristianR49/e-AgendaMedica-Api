using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_AgendaMedica.Dominio.Compartilhado
{
    public interface IRepositorioBase<TEntidade> where TEntidade : Entidade
    {
        void Editar(TEntidade registro);
        void Excluir(TEntidade registro);
        Task<bool> InserirAsync(TEntidade registro);
        Task<TEntidade> SelecionarPorIdAsync(Guid id);
        Task<List<TEntidade>> SelecionarTodosAsync();
    }
}
