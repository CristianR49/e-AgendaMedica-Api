using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using e_AgendaMedica.Infra.Orm.ModuloAtividade;
using FluentResults;

namespace eAgendaMedica.Aplicacao.ModuloMedico
{
    public class ServicoMedico
    {
        public readonly IRepositorioMedico repositorioMedico;
        private readonly IContextoPersistencia contextoPersistencia;
        private readonly IRepositorioAtividade repositorioAtividade;

        public ServicoMedico(IRepositorioMedico repositorioMedico, IContextoPersistencia contextoPersistencia, IRepositorioAtividade repositorioAtividade)
        {
            this.repositorioMedico = repositorioMedico;
            this.contextoPersistencia = contextoPersistencia;
            this.repositorioAtividade = repositorioAtividade;
        }

        public async Task<Result<Medico>> InserirAsync(Medico medico)
        {
            var resultadoValidacao = ValidarMedico(medico);

            Result crmRepetido = TestarCrmRepetido(medico.Crm);

            if (crmRepetido.IsFailed)
                return crmRepetido;

            if (resultadoValidacao.IsFailed)
                return Result.Fail(resultadoValidacao.Errors);

            await this.repositorioMedico.InserirAsync(medico);

            await contextoPersistencia.GravarAsync();

            return Result.Ok(medico);
        }

        private Result TestarCrmRepetido(string crmCriado)
        {
            foreach (var medico in repositorioMedico.SelecionarTodosAsync().Result)
            {
                if (medico.Crm == crmCriado)
                {
                    return Result.Fail("Esse CRM já está registrado");
                }
            }
            return Result.Ok();
        }

        public async Task<Result<Medico>> EditarAsync(Medico medico)
        {
            var resultadoValidacao = ValidarMedico(medico);

            Result crmRepetido = TestarCrmRepetido(medico.Crm);

            if (crmRepetido.IsFailed)
                return crmRepetido;

            if (resultadoValidacao.IsFailed)
                return Result.Fail(resultadoValidacao.Errors);

            if (resultadoValidacao.IsFailed)
                return Result.Fail(resultadoValidacao.Errors);

            repositorioMedico.Editar(medico);

            await contextoPersistencia.GravarAsync();

            return Result.Ok(medico);
        }

        public async Task<Result> ExcluirAsync(Guid id)
        {
            var medico = await repositorioMedico.SelecionarPorIdAsync(id);

            bool estaRegistrado = VerificarSeEstaEmAtividade(medico);

            if (estaRegistrado)
            {
                return Result.Fail("Este médico está registrado em uma atividade!");
            }

            repositorioMedico.Excluir(medico);

            await contextoPersistencia.GravarAsync();

            return Result.Ok();
        }

        private bool VerificarSeEstaEmAtividade(Medico medico)
        {
            List<Atividade> atividades = repositorioAtividade.SelecionarTodosAsync().Result;

            bool estaRegistrado = false;

            foreach (var atividade in atividades)
            {
                if (atividade.Medicos.Contains(medico))
                {
                    estaRegistrado = true;
                }
            }

            return estaRegistrado;
        }

        public async Task<Result<List<Medico>>> SelecionarTodosAsync()
        {
            var medicos = await repositorioMedico.SelecionarTodosAsync();

            return Result.Ok(medicos);
        }

        public async Task<Result<Medico>> SelecionarPorIdAsync(Guid id)
        {
            var medico = await repositorioMedico.SelecionarPorIdAsync(id);

            return Result.Ok(medico);
        }

        private Result ValidarMedico(Medico medico)
        {
            ValidadorMedico validador = new ValidadorMedico();

            var resultadoValidacao = validador.Validate(medico);

            List<Error> erros = new List<Error>();

            foreach (var erro in resultadoValidacao.Errors)
            {
                erros.Add(new Error(erro.ErrorMessage));
            }

            if (erros.Any())
                return Result.Fail(erros.ToArray());

            return Result.Ok();
        }
    }
}
