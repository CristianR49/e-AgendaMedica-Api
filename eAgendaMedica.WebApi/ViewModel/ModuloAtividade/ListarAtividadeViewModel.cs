using e_AgendaMedica.Dominio.ModuloAtividade;

namespace eAgendaMedica.WebApi.ViewModel.ModuloAtividade
{
    public class ListarAtividadeViewModel
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioTermino { get; set; }
        public TipoAtividadeEnum TipoAtividade { get; set; }

    }
}
