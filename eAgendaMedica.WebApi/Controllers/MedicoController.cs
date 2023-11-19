using AutoMapper;
using eAgendaMedica.Aplicacao.ModuloMedico;
using Microsoft.AspNetCore.Mvc;
using static eAgendaMedica.WebApi.ViewModel.ModuloMedico.MedicoViewModel;

namespace eAgendaMedica.WebApi.Controllers
{
    [Route("api/medicos")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly ServicoMedico servicoMedico;
        private readonly IMapper mapeador;

        public MedicoController(ServicoMedico servicoMedico, IMapper mapeador)
        {
            this.servicoMedico = servicoMedico;
            this.mapeador = mapeador;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var medicos = await servicoMedico.SelecionarTodosAsync();

            var viewModel = mapeador.Map<List<ListarMedicoViewModel>>(medicos.Value);

            return Ok(viewModel);
        }
    }
}
