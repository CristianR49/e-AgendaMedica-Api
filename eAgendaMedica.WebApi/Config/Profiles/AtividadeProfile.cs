using AutoMapper;
using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using e_AgendaMedica.Infra.Orm.ModuloMedico;
using eAgendaMedica.WebApi.ViewModel.ModuloAtividade;

namespace eAgendaMedica.WebApi.Config.Profiles
{
    public class AtividadeProfile : Profile
    {
        public AtividadeProfile()
        {
            CreateMap<Atividade, ListarAtividadeViewModel>()
                .ForMember(destino => destino.Data, opt => opt.MapFrom(origem => origem.Data.ToShortDateString()))
                .ForMember(destino => destino.HorarioInicio, opt => opt.MapFrom(origem => origem.HorarioInicio.ToString(@"hh\:mm")))
                .ForMember(destino => destino.HorarioTermino, opt => opt.MapFrom(origem => origem.HorarioTermino.ToString(@"hh\:mm")))
                .ForMember(destino => destino.NomesMedicos, opt => opt.MapFrom(origem => origem.Medicos.Select(x => x.Nome).ToList()));

            CreateMap<InserirAtividadeViewModel, Atividade>()
                .ForMember(destino => destino.Data, opt => opt.MapFrom(origem => origem.Data.ToShortDateString()))
                .ForMember(destino => destino.HorarioInicio, opt => opt.MapFrom(origem => origem.HorarioInicio.ToString(@"hh\:mm")))
                .ForMember(destino => destino.HorarioTermino, opt => opt.MapFrom(origem => origem.HorarioTermino.ToString(@"hh\:mm")))
                .ForMember(destino => destino.TipoAtividade, opt => opt.MapFrom(origem => (TipoAtividadeEnum)origem.TipoAtividade))
                .ForMember(destino => destino.Medicos, opt => opt.Ignore())
                .AfterMap<FormsMedicosMappingAction>();

            CreateMap<InserirAtividadeViewModel, Atividade>()
                .ForMember(destino => destino.Medicos, opt => opt.Ignore())
                .AfterMap<FormsMedicosMappingAction>();

            CreateMap<EditarAtividadeViewModel, Atividade>()
                .ForMember(destino => destino.Medicos, opt => opt.Ignore())
                .AfterMap<FormsMedicosMappingAction>();
        }
    }

    public class FormsMedicosMappingAction : IMappingAction<FormsAtividadeViewModel, Atividade>
    {
        private readonly IRepositorioMedico repositorioMedico;

        public FormsMedicosMappingAction(IRepositorioMedico repositorioMedico)
        {
            this.repositorioMedico = repositorioMedico;
        }

        public void Process(FormsAtividadeViewModel source, Atividade destination, ResolutionContext context)
        {
            destination.Medicos = repositorioMedico.SelecionarMuitos(source.MedicosSelecionados);
        }
    }
}
