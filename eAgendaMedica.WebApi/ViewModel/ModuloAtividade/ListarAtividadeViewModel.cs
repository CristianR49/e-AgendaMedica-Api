using e_AgendaMedica.Dominio.ModuloAtividade;

namespace eAgendaMedica.WebApi.ViewModel.ModuloAtividade
{
    public class ListarAtividadeViewModel
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataConclusao { get; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioTermino { get; set; }
        public TipoAtividadeEnum TipoAtividade { get; set; }
        public List<string> NomesMedicos { get; set; }

    }
}
