using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Infra.Orm.Compartilhado;
using e_AgendaMedica.Infra.Orm.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_AgendaMedica.Infra.Orm.ModuloAtividade
{
    public class RepositorioAtividadeOrm : RepositorioBase<Atividade>, IRepositorioAtividade
    {
        public RepositorioAtividadeOrm(IContextoPersistencia dbContext) : base(dbContext)
        {
            
        }

        public override async Task<Atividade> SelecionarPorIdAsync(Guid id)
        {
            return await registros.Include(x => x.Medicos)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<List<Atividade>> SelecionarTodosAsync()
        {
            return await registros.Include(x => x.Medicos)
                .ToListAsync();
        }
    }
}
