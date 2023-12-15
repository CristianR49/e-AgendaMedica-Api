using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using eAgendaMedica.Aplicacao.ModuloAtividade;
using FluentResults.Extensions.FluentAssertions;
using FluentValidation.Results;
using Moq;

namespace eAgendaMedica.TestesUnitarios.Aplicacao.ModuloAtividade
{
    [TestClass]
    public class ServicoAtividadeTest
    {
        Mock<IContextoPersistencia> contextoMoq;
        Mock<IValidadorAtividade> validadorMoq;
        Mock<IRepositorioAtividade> repositorioAtividadeMoq;
        Mock<IContextoPersistencia> ContextoPersistenciaMoq { get; set; }
        ServicoAtividade servicoAtividade;

        Atividade atividade;
        List<Medico> medicos;

        public ServicoAtividadeTest()
        {
            repositorioAtividadeMoq = new Mock<IRepositorioAtividade>();
            ContextoPersistenciaMoq = new Mock<IContextoPersistencia>();
            validadorMoq = new Mock<IValidadorAtividade>();
            servicoAtividade = new ServicoAtividade(repositorioAtividadeMoq.Object, ContextoPersistenciaMoq.Object, validadorMoq.Object);
            medicos = new List<Medico>();
            medicos.Add(new Medico("João", "4444-TM"));
            atividade = new Atividade(new DateTime(1555, 5, 20), new TimeSpan(20, 0, 0), new TimeSpan(22, 0, 0), TipoAtividadeEnum.Cirurgia, medicos);
        }

        [TestMethod]
        public async Task Deve_inserir_atividade_caso_ela_for_valida()
        {
            //arrange
            atividade = new Atividade(new DateTime(1555, 5, 20), new TimeSpan(20, 0, 0), new TimeSpan(22, 0, 0), TipoAtividadeEnum.Cirurgia, medicos);

            //action
            var resultado = await servicoAtividade.InserirAsync(atividade);

            //assert 
            resultado.Should().BeSuccess();
            repositorioAtividadeMoq.Verify(x => x.InserirAsync(atividade), Times.Once());
        }

        [TestMethod]
        public async Task Nao_deve_inserir_atividade_caso_ela_seja_invalida()
        {
            //arrange
            validadorMoq.Setup(x => x.Validate(It.IsAny<Atividade>()))
                .Returns(() =>
                {
                    var resultado = new ValidationResult();
                    resultado.Errors.Add(new ValidationFailure("Data", "Data de conclusão não deve ser antes da Data"));
                    return resultado;
                });

            //action
            var resultado = await servicoAtividade.InserirAsync(atividade);

            //assert             
            resultado.Should().BeFailure();
            repositorioAtividadeMoq.Verify(x => x.InserirAsync(atividade), Times.Never());
        }

        [TestMethod]
        public async Task Nao_deve_inserir_atividade_caso_medico_ja_esteja_com_choque_de_horario()
        {
            //arrange

            repositorioAtividadeMoq.Setup(x => x.SelecionarTodos())
                .Returns(() =>
                {
                    var atividades = new List<Atividade>();
                    atividades.Add(new Atividade(new DateTime(1555, 5, 20), new TimeSpan(19, 0, 0), new TimeSpan(21, 0, 0), TipoAtividadeEnum.Cirurgia, medicos));
                    return atividades;
                });

            //action
            var resultado = await servicoAtividade.InserirAsync(atividade);

            //assert 
            resultado.Should().BeFailure();
            repositorioAtividadeMoq.Verify(x => x.InserirAsync(atividade), Times.Never());
        }

        [TestMethod]
        public async Task Nao_deve_inserir_atividade_caso_medico_ja_esteja_com_choque_de_descanco_antes()
        {
            //arrange

            repositorioAtividadeMoq.Setup(x => x.SelecionarTodos())
                .Returns(() =>
                {
                    var atividades = new List<Atividade>();
                    atividades.Add(new Atividade(new DateTime(1555, 5, 20), new TimeSpan(19, 0, 0), new TimeSpan(21, 0, 0), TipoAtividadeEnum.Consulta, medicos));
                    return atividades;
                });

            var atividadeCriada = new Atividade(new DateTime(1555, 5, 20), new TimeSpan(18, 0, 0), new TimeSpan(18, 50, 0), TipoAtividadeEnum.Consulta, medicos);

            //action
            var resultado = await servicoAtividade.InserirAsync(atividadeCriada);

            //assert 
            resultado.Should().BeFailure();
            repositorioAtividadeMoq.Verify(x => x.InserirAsync(atividadeCriada), Times.Never());
        }

        [TestMethod]
        public async Task Nao_deve_inserir_atividade_caso_medico_ja_esteja_com_choque_de_descanco_depois()
        {
            //arrange

            repositorioAtividadeMoq.Setup(x => x.SelecionarTodos())
                .Returns(() =>
                {
                    var atividades = new List<Atividade>();
                    atividades.Add(new Atividade(new DateTime(1555, 5, 20), new TimeSpan(19, 0, 0), new TimeSpan(21, 0, 0), TipoAtividadeEnum.Consulta, medicos));
                    return atividades;
                });

            var atividadeCriada = new Atividade(new DateTime(1555, 5, 20), new TimeSpan(21, 15, 0), new TimeSpan(22, 00, 0), TipoAtividadeEnum.Consulta, medicos);

            //action
            var resultado = await servicoAtividade.InserirAsync(atividadeCriada);

            //assert 
            resultado.Should().BeFailure();
            repositorioAtividadeMoq.Verify(x => x.InserirAsync(atividadeCriada), Times.Never());
        }

        [TestMethod]
        public async Task Deve_editar_atividade_caso_ela_for_valida()
        {
            //arrange           
            atividade = new Atividade(new DateTime(1555, 5, 20), new TimeSpan(20, 0, 0), new TimeSpan(22, 0, 0), TipoAtividadeEnum.Cirurgia, medicos);

            //action
            var resultado = await servicoAtividade.EditarAsync(atividade);

            //assert 
            resultado.Should().BeSuccess();
            repositorioAtividadeMoq.Verify(x => x.Editar(atividade), Times.Once());
        }


        [TestMethod]
        public async Task Nao_deve_editar_atividade_caso_ela_seja_invalida()
        {
            //arrange
            validadorMoq.Setup(x => x.Validate(It.IsAny<Atividade>()))
                .Returns(() =>
                {
                    var resultado = new ValidationResult();
                    resultado.Errors.Add(new ValidationFailure("Data", "Data de conclusão não deve ser antes da Data"));
                    return resultado;
                });

            //action
            var resultado = await servicoAtividade.EditarAsync(atividade);

            //assert             
            resultado.Should().BeFailure();
            repositorioAtividadeMoq.Verify(x => x.Editar(atividade), Times.Never());
        }

        [TestMethod]
        public async Task Deve_excluir_atividade_caso_ela_esteja_cadastrada()
        {
            Guid id = Guid.NewGuid();
            //arrange
            var atividade = new Atividade(id, new DateTime(1555, 5, 20), new TimeSpan(20, 0, 0), new TimeSpan(22, 0, 0), TipoAtividadeEnum.Cirurgia, medicos);

            repositorioAtividadeMoq.Setup(x => x.SelecionarPorId(atividade.Id))
               .Returns(() =>
               {
                   return atividade;
               });

            repositorioAtividadeMoq.Setup(x => x.Existe(atividade))
               .Returns(() =>
               {
                   return true;
               });

            //action
            var resultado = await servicoAtividade.ExcluirAsync(atividade.Id);

            //assert 
            resultado.Should().BeSuccess();
            repositorioAtividadeMoq.Verify(x => x.Excluir(atividade), Times.Once());
        }

        [TestMethod]
        public async Task Nao_deve_excluir_atividade_caso_ela_nao_esteja_cadastrada()
        {
            //arrange

            var atividade = new Atividade(new DateTime(1555, 5, 20), new TimeSpan(20, 0, 0), new TimeSpan(22, 0, 0), TipoAtividadeEnum.Cirurgia, medicos);

            repositorioAtividadeMoq.Setup(x => x.Existe(atividade))
               .Returns(() =>
               {
                   return false;
               });

            //action
            var resultado = await servicoAtividade.ExcluirAsync(atividade.Id);

            //assert 
            resultado.Should().BeFailure();
            repositorioAtividadeMoq.Verify(x => x.Excluir(atividade), Times.Never());
        }
    }
}
