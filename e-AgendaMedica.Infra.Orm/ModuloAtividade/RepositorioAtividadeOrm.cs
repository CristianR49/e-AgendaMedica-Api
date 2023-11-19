using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Infra.Orm.Compartilhado;
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
    }
}
