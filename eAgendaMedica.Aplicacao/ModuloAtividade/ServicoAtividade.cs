using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloAtividade;
using FluentResults;

namespace eAgendaMedica.Aplicacao.ModuloAtividade
{
    public class ServicoAtividade
    {
        public readonly IRepositorioAtividade repositorioAtividade;
        private readonly IContextoPersistencia contextoPersistencia;
        private readonly IValidadorAtividade validador;

        public ServicoAtividade(IRepositorioAtividade repositorioAtividade, IContextoPersistencia contextoPersistencia, IValidadorAtividade validador)
        {
            this.repositorioAtividade = repositorioAtividade;
            this.contextoPersistencia = contextoPersistencia;
            this.validador = validador;
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
            var atividade = repositorioAtividade.SelecionarPorId(id);

            if (repositorioAtividade.Existe(atividade))
            {
                repositorioAtividade.Excluir(atividade);

                await contextoPersistencia.GravarAsync();

                return Result.Ok();
            }
            else
            {
                return Result.Fail("Essa atividade não existe!");
            }
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
            var resultadoValidacao = validador.Validate(atividade);

            List<Error> erros = new List<Error>();

            if (resultadoValidacao != null)
            {
                foreach (var erro in resultadoValidacao.Errors)
                {
                    erros.Add(new Error(erro.ErrorMessage));
                }
            }

            Result resultadoApenasUmMedico = ConsultaTemNoMaximoUmMedico(atividade);

            if (resultadoApenasUmMedico.IsFailed)
                erros.Add(new Error(resultadoApenasUmMedico.Errors[0].Message));

            Result resultadoChoqueHorarios = ValidarHorarios(atividade);

            if (resultadoChoqueHorarios.IsFailed)
                erros.Add(new Error(resultadoChoqueHorarios.Errors[0].Message));

            if (erros.Any())
                return Result.Fail(erros.ToArray());

            return Result.Ok();
        }

        private Result ConsultaTemNoMaximoUmMedico(Atividade atividadeCriada)
        {
            if (atividadeCriada.TipoAtividade == TipoAtividadeEnum.Consulta && atividadeCriada.Medicos.Count > 1)
            {
                return Result.Fail("Uma consulta pode ter apenas um médico");
            }
            return Result.Ok();
        }

        private Result ValidarHorarios(Atividade atividadeCriada)
        {
            var atividades = repositorioAtividade.SelecionarTodos();

            if (atividades != null)
            {
                foreach (var atividade in atividades)
                {
                    foreach (var medico in atividadeCriada.Medicos)
                    {
                        if (atividade.Medicos.Contains(medico))
                        {

                            bool mesmoDia = atividadeCriada.Data.Date == atividade.Data.Date;


                            bool horariosMarcadosChocam =
                            atividadeCriada.HorarioInicio >= atividade.HorarioInicio
                            && atividadeCriada.HorarioInicio <= atividade.HorarioTermino
                            || atividadeCriada.HorarioTermino >= atividade.HorarioInicio
                            && atividadeCriada.HorarioTermino <= atividade.HorarioTermino
                            || atividadeCriada.HorarioInicio <= atividade.HorarioInicio
                            && atividadeCriada.HorarioTermino >= atividade.HorarioTermino;

                            if (mesmoDia && horariosMarcadosChocam)
                            {
                                return Result.Fail($"O médico {medico.Nome} já está marcado para esse horário! Data: {atividade.Data.Date} ({atividade.HorarioInicio} - {atividade.HorarioTermino})");
                            }

                            //

                            TimeSpan tempoDescancoAtividade = TimeSpan.Zero;

                            if (atividade.TipoAtividade == TipoAtividadeEnum.Cirurgia)
                            {
                                tempoDescancoAtividade = TimeSpan.Parse("4:00");
                            }
                            else if (atividade.TipoAtividade == TipoAtividadeEnum.Consulta)
                            {
                                tempoDescancoAtividade = TimeSpan.Parse("00:20");
                            }

                            TimeSpan descancoDepoisDoTerminoAtividade = atividade.HorarioTermino + tempoDescancoAtividade;

                            
                            TimeSpan tempoDescancoAtividadeCriada = TimeSpan.Zero;

                            if (atividade.TipoAtividade == TipoAtividadeEnum.Cirurgia)
                            {
                                tempoDescancoAtividadeCriada = TimeSpan.Parse("4:00");
                            }
                            else if (atividade.TipoAtividade == TipoAtividadeEnum.Consulta)
                            {
                                tempoDescancoAtividadeCriada = TimeSpan.Parse("00:20");
                            }

                            TimeSpan descancoDepoisDoTerminoAtividadeCriada = atividadeCriada.HorarioTermino + tempoDescancoAtividadeCriada;

                            //

                            bool atividadeCriadaDescancoChoca =
                                atividade.HorarioInicio <= descancoDepoisDoTerminoAtividadeCriada
                                && descancoDepoisDoTerminoAtividadeCriada <= atividade.HorarioTermino
                                || atividade.HorarioTermino <= descancoDepoisDoTerminoAtividadeCriada
                                && descancoDepoisDoTerminoAtividadeCriada <= descancoDepoisDoTerminoAtividade
                                || descancoDepoisDoTerminoAtividade <= descancoDepoisDoTerminoAtividadeCriada;

                            //

                            if (mesmoDia && atividadeCriada.HorarioTermino < atividade.HorarioInicio && atividadeCriadaDescancoChoca)
                            {
                                return Result.Fail($"O médico {medico.Nome} não terminaria seu descanço as {descancoDepoisDoTerminoAtividadeCriada} antes de outra atividade começar as {atividade.HorarioInicio}");
                            }

                            //

                            bool atividadeDescancoChoca = 
                                atividadeCriada.HorarioTermino >= descancoDepoisDoTerminoAtividade
                                || descancoDepoisDoTerminoAtividade >= atividadeCriada.HorarioTermino;

                            if (mesmoDia && atividadeCriada.HorarioInicio <= descancoDepoisDoTerminoAtividade && atividadeDescancoChoca)
                            {
                                return Result.Fail($"O médico {medico.Nome} estará descançando nesse horário e voltará a ativa as {atividade.HorarioTermino + tempoDescancoAtividade + TimeSpan.Parse("00:01")}");
                            }

                        }
                    }
                }
            }
            return Result.Ok();
        }
    }
}
