using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using FluentResults;

namespace eAgendaMedica.Aplicacao.ModuloMedico
{
    public class ServicoMedico
    {
        public readonly IRepositorioMedico repositorioMedico;
        private readonly IContextoPersistencia contextoPersistencia;
        private readonly IRepositorioAtividade repositorioAtividade;
        private readonly IValidadorMedico validador;

        public ServicoMedico(IRepositorioMedico repositorioMedico, IContextoPersistencia contextoPersistencia, IRepositorioAtividade repositorioAtividade, IValidadorMedico validador)
        {
            this.repositorioMedico = repositorioMedico;
            this.contextoPersistencia = contextoPersistencia;
            this.repositorioAtividade = repositorioAtividade;
            this.validador = validador;
        }

        public async Task<Result<Medico>> InserirAsync(Medico medico)
        {
            var resultadoValidacao = ValidarMedico(medico);

            if (resultadoValidacao.IsFailed)
                return Result.Fail(resultadoValidacao.Errors);

            await this.repositorioMedico.InserirAsync(medico);

            await contextoPersistencia.GravarAsync();

            return Result.Ok(medico);
        }

        private Result TestarCrmRepetido(Medico medico)
        {
            Medico? medicoEncontrado = repositorioMedico.SelecionarPorCrm(medico.Crm);

            if (medicoEncontrado != null &&
                medicoEncontrado.Id != medico.Id &&
                medicoEncontrado.Crm == medico.Crm)
            {
                return Result.Fail("Esse CRM já está sendo usado por um médico");
            }

            return Result.Ok();
        }

        public async Task<Result<Medico>> EditarAsync(Medico medico)
        {
            var resultadoValidacao = ValidarMedico(medico);

            if (resultadoValidacao.IsFailed)
                return Result.Fail(resultadoValidacao.Errors);

            repositorioMedico.Editar(medico);

            await contextoPersistencia.GravarAsync();

            return Result.Ok(medico);
        }

        public async Task<Result> ExcluirAsync(Guid id)
        {
            var medico = repositorioMedico.SelecionarPorId(id);

            if (repositorioMedico.Existe(medico))
            {

                var resultadoValidacao = VerificarSeEstaEmAtividade(medico);

                if (resultadoValidacao.IsFailed)
                    return Result.Fail(resultadoValidacao.Errors);


                repositorioMedico.Excluir(medico);

                await contextoPersistencia.GravarAsync();

                return Result.Ok();
            }
            else 
            {
                return Result.Fail("Esse médico não existe!");
            }
        }

        private Result VerificarSeEstaEmAtividade(Medico medico)
        {
            List<Atividade> atividades = repositorioAtividade.SelecionarTodos();

            if (atividades != null)
            {
                foreach (var atividade in atividades)
                {
                    if (atividade.Medicos.Contains(medico))
                    {
                        return Result.Fail("Este médico está registrado em uma atividade!");
                    }
                }
            }

            return Result.Ok();
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

            var resultadoValidacao = validador.Validate(medico);

            List<IError> erros = new List<IError>();

            if (resultadoValidacao != null)
            {
                foreach (var erro in resultadoValidacao.Errors)
                {
                    erros.Add(new Error(erro.ErrorMessage));
                }
            }

            Result resultado = TestarCrmRepetido(medico);

            if (resultado.IsFailed)
                erros.Add(resultado.Errors[0]);

            if (erros.Any())
                return Result.Fail(erros.ToArray());

            return Result.Ok();
        }
    }
}
