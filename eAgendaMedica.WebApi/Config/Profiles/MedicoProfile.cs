using AutoMapper;
using e_AgendaMedica.Dominio.ModuloMedico;
using static eAgendaMedica.WebApi.ViewModel.ModuloMedico.MedicoViewModel;

namespace eAgendaMedica.WebApi.Config.Profiles
{
    public class MedicoProfile : Profile
    {
        public MedicoProfile()
        {
            CreateMap<Medico, ListarMedicoViewModel>();
            CreateMap<Medico, VisualizarMedicoViewModel>();
            CreateMap<InserirMedicoViewModel, Medico>();
        }
    }
}