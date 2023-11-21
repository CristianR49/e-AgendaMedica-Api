using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloAtividade;
using FluentResults;

namespace eAgendaMedica.Aplicacao.ModuloAtividade
{
    public class ServicoAtividade
    {
        public readonly IRepositorioAtividade repositorioAtividade;
        private readonly IContextoPersistencia contextoPersistencia;

        public ServicoAtividade(IRepositorioAtividade repositorioAtividade, IContextoPersistencia contextoPersistencia)
        {
            this.repositorioAtividade = repositorioAtividade;
            this.contextoPersistencia = contextoPersistencia;
        }

        public async Task<Result<Atividade>> InserirAsync(Atividade atividade)
        {
            var resultadoValidacao = ValidarAtividade(atividade);

            if (resultadoValidacao.IsFailed)
                return Result.Fail(resultadoValidacao.Errors);

            await this.repositorioAtividade.InserirAsync(atividade);

            await contextoPersistencia.GravarAsync();

            return Result.Ok(atividade);
        }

        public async Task<Result<Atividade>> EditarAsync(Atividade atividade)
        {
            var resultadoValidacao = ValidarAtividade(atividade);

            if (resultadoValidacao.IsFailed)
                return Result.Fail(resultadoValidacao.Errors);

            repositorioAtividade.Editar(atividade);

            await contextoPersistencia.GravarAsync();

            return Result.Ok(atividade);
        }

        public async Task<Result> ExcluirAsync(Guid id)
        {
            var atividade = await repositorioAtividade.SelecionarPorIdAsync(id);

            repositorioAtividade.Excluir(atividade);

            await contextoPersistencia.GravarAsync();

            return Result.Ok();
        }

        public async Task<Result<List<Atividade>>> SelecionarTodosAsync()
        {
            var atividades = await repositorioAtividade.SelecionarTodosAsync();

            return Result.Ok(atividades);
        }

        public async Task<Result<Atividade>> SelecionarPorIdAsync(Guid id)
        {
            var atividade = await repositorioAtividade.SelecionarPorIdAsync(id);

            return Result.Ok(atividade);
        }

        private Result ValidarAtividade(Atividade atividade)
        {
            ValidadorAtividade validador = new ValidadorAtividade();

            var resultadoValidacao = validador.Validate(atividade);

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
