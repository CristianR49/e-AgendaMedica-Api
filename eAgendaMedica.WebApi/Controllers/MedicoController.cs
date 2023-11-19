using AutoMapper;
using e_AgendaMedica.Dominio.ModuloMedico;
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
        public async Task<IActionResult> SelecionarTodos()
        {
            var medicosResult = await servicoMedico.SelecionarTodosAsync();

            var viewModel = mapeador.Map<List<ListarMedicoViewModel>>(medicosResult.Value);

            return Ok(viewModel);
        }

        [HttpGet("visualizacao-completa/{id}")]
        public async Task<IActionResult> SelecionarPorId(Guid id)
        {
            var medicoResult = await servicoMedico.SelecionarPorIdAsync(id);

            var viewModel = mapeador.Map<VisualizarMedicoViewModel>(medicoResult.Value);

            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Inserir(InserirMedicoViewModel viewModel)
        {
            var medico = mapeador.Map<Medico>(viewModel);

            await servicoMedico.InserirAsync(medico);

            return Ok(viewModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(Guid id, InserirMedicoViewModel viewModel)
        {
            var medicoResult = await servicoMedico.SelecionarPorIdAsync(id);

            var medico = mapeador.Map(viewModel, medicoResult.Value);

            await servicoMedico.EditarAsync(medico);

            return Ok(viewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            await servicoMedico.ExcluirAsync(id);

            return Ok();
        }
    }
}
