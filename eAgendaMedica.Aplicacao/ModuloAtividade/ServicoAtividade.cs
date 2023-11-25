﻿using e_AgendaMedica.Dominio.Compartilhado;
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

            Result resultadoApenasUmMedico = ConsultaTemNoMinimoUmMedico(atividade);

            if (resultadoApenasUmMedico.IsFailed)
                return resultadoApenasUmMedico;

            Result resultadoChoqueHorarios = ValidarHorarios(atividade);

            if (resultadoChoqueHorarios.IsFailed)
                return resultadoChoqueHorarios;


            await this.repositorioAtividade.InserirAsync(atividade);

            await contextoPersistencia.GravarAsync();

            return Result.Ok(atividade);
        }

        private Result ConsultaTemNoMinimoUmMedico(Atividade atividadeCriada)
        {
            if (atividadeCriada.TipoAtividade == TipoAtividadeEnum.Consulta && atividadeCriada.Medicos.Count > 1)
            {
                return Result.Fail("Uma consulta pode ter apenas um médico");
            }
            return Result.Ok();
        }

        private Result ValidarHorarios(Atividade atividadeCriada)
        {
            if ((atividadeCriada.DataConclusao.Date - atividadeCriada.Data.Date).Days > 1)
            {
                return Result.Fail("Uma atividade não pode começar e acabar dois dias depois");
            }

            var atividades = repositorioAtividade.SelecionarTodos();

            if (atividades != null)
            {
                foreach (var atividade in atividades)
                {
                    foreach (var medico in atividadeCriada.Medicos)
                    {
                        if (atividade.Medicos.Contains(medico))
                        {
                            bool mesmoDia = atividadeCriada.Data == atividade.Data
                                && atividadeCriada.DataConclusao == atividade.DataConclusao
                                && atividadeCriada.DataConclusao == atividadeCriada.Data;

                            bool horariosMarcadosChocam =
                            atividade.HorarioInicio <= atividadeCriada.HorarioInicio
                            && atividade.HorarioTermino >= atividadeCriada.HorarioInicio
                            || atividade.HorarioInicio <= atividadeCriada.HorarioTermino
                            && atividade.HorarioTermino >= atividadeCriada.HorarioTermino
                            || atividade.HorarioInicio >= atividadeCriada.HorarioInicio
                            && atividade.HorarioTermino <= atividadeCriada.HorarioTermino
                            || atividadeCriada.DataConclusao == atividadeCriada.Data
                            && atividade.HorarioInicio <= atividadeCriada.HorarioInicio
                            && atividade.HorarioTermino >= atividadeCriada.HorarioTermino;

                            if (mesmoDia && horariosMarcadosChocam)
                            {
                                return Result.Fail($"O médico {medico.Nome} já está marcado para esse horário! Data: {atividade.Data} ({atividade.HorarioInicio} - {atividade.HorarioTermino})");
                            }


                            TimeSpan tempoDescanco = TimeSpan.Parse("00:20");

                            if (atividade.TipoAtividade == TipoAtividadeEnum.Cirurgia)
                            {
                                tempoDescanco = TimeSpan.Parse("4:00");
                            }

                            bool horarioDescancoChoca = atividade.HorarioTermino + tempoDescanco <= atividadeCriada.HorarioInicio;

                            if (horarioDescancoChoca)
                            {
                                return Result.Fail($"O médico {medico.Nome} estará descançando nesse horário e voltará a ativa as {atividade.HorarioTermino + tempoDescanco + TimeSpan.Parse("00:01")}");
                            }

                        }
                    }
                }
            }
            return Result.Ok();
        }

        public async Task<Result<Atividade>> EditarAsync(Atividade atividade)
        {
            var resultadoValidacao = ValidarAtividade(atividade);

            if (resultadoValidacao.IsFailed)
                return Result.Fail(resultadoValidacao.Errors);

            Result resultadoApenasUmMedico = ConsultaTemNoMinimoUmMedico(atividade);

            if (resultadoApenasUmMedico.IsFailed)
                return resultadoApenasUmMedico;

            Result resultadoChoqueHorarios = ValidarHorarios(atividade);

            if (resultadoChoqueHorarios.IsFailed)
                return resultadoChoqueHorarios;

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

            if (erros.Any())
                return Result.Fail(erros.ToArray());

            return Result.Ok();
        }
    }
}
