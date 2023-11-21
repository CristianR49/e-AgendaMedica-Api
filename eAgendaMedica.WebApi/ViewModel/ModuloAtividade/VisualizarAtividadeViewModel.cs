using e_AgendaMedica.Dominio.ModuloAtividade;
using static eAgendaMedica.WebApi.ViewModel.ModuloMedico.MedicoViewModel;

namespace eAgendaMedica.WebApi.ViewModel.ModuloAtividade
{
    public class VisualizarAtividadeViewModel
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioTermino { get; set; }
        public TipoAtividadeEnum TipoAtividade { get; set; }
        public List<ListarMedicoViewModel> Medicos { get; set; }

    }
}
