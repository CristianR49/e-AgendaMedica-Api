using AutoMapper;
using e_AgendaMedica.Dominio.ModuloAtividade;
using eAgendaMedica.Aplicacao.ModuloAtividade;
using eAgendaMedica.WebApi.ViewModel.ModuloAtividade;
using Microsoft.AspNetCore.Mvc;

namespace eAgendaMedica.WebApi.Controllers
{
    [Route("api/atividades")]
    [ApiController]
    public class AtividadeController : ControllerBase
    {
        private readonly ServicoAtividade servicoAtividade;
        private readonly IMapper mapeador;

        public AtividadeController(ServicoAtividade servicoAtividade, IMapper mapeador)
        {
            this.servicoAtividade = servicoAtividade;
            this.mapeador = mapeador;
        }

        [HttpGet]
        public async Task<IActionResult> SelecionarTodos()
        {
            var atividadesResult = await servicoAtividade.SelecionarTodosAsync();

            var viewModel = mapeador.Map<List<ListarAtividadeViewModel>>(atividadesResult.Value);

            return Ok(viewModel);
        }

        [HttpGet("visualizacao-completa/{id}")]
        public async Task<IActionResult> SelecionarPorId(Guid id)
        {
            var atividadeResult = await servicoAtividade.SelecionarPorIdAsync(id);

            var viewModel = mapeador.Map<VisualizarAtividadeViewModel>(atividadeResult.Value);

            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Inserir(InserirAtividadeViewModel viewModel)
        {
            var atividade = mapeador.Map<Atividade>(viewModel);

            await servicoAtividade.InserirAsync(atividade);

            return Ok(viewModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(Guid id, EditarAtividadeViewModel viewModel)
        {
            var atividadeResult = await servicoAtividade.SelecionarPorIdAsync(id);

            var atividade = mapeador.Map(viewModel, atividadeResult.Value);

            await servicoAtividade.EditarAsync(atividade);

            return Ok(viewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            await servicoAtividade.ExcluirAsync(id);

            return Ok();
        }
    }
}
