using eAgendaMedica.Aplicacao.ModuloMedico;
using Microsoft.AspNetCore.Mvc;

namespace eAgendaMedica.WebApi.Controllers
{
    public class MedicoController : ControllerBase
    {
        private readonly ServicoMedico servicoMedico;

        public MedicoController(ServicoMedico servicoMedico)
        {
            this.servicoMedico = servicoMedico;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var medicos = await servicoMedico.SelecionarTodosAsync();

            return Ok();
        }
    }
}
