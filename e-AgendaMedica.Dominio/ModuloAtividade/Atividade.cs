using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloMedico;

namespace e_AgendaMedica.Dominio.ModuloAtividade
{
    public class Atividade : Entidade
    {
        public DateTime Data { get; set; }
        public DateTime DataConclusao { get; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioTermino { get; set; }
        public TipoAtividadeEnum TipoAtividade { get; set; }
        public List<Medico> Medicos { get; set; }
        public Atividade(DateTime data, DateTime dataConclusao, TimeSpan horarioInicio, TimeSpan horarioTermino, TipoAtividadeEnum tipoAtividade, List<Medico> medicos)
        {
            Data = data;
            DataConclusao = dataConclusao;
            HorarioInicio = horarioInicio;
            HorarioTermino = horarioTermino;
            TipoAtividade = tipoAtividade;
            Medicos = medicos;
        }
        public Atividade()
        {

        }
    }
}
