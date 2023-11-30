using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using eAgendaMedica.Aplicacao.ModuloMedico;
using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using FluentValidation.Results;
using Moq;

namespace eAgendaMedica.TestesUnitarios.Aplicacao
{
    [TestClass]
    public class ServicoMedicoTest
    {
        Mock<IRepositorioMedico> repositorioMedicoMoq;
        Mock<IContextoPersistencia> contextoMoq;
        Mock<IValidadorMedico> validadorMoq;
        Mock<IRepositorioAtividade> repositorioAtividadeMoq;
        Mock<IContextoPersistencia> ContextoPersistenciaMoq { get; set; }
        ServicoMedico servicoMedico;

        Medico medico;

        public ServicoMedicoTest()
        {
            repositorioAtividadeMoq = new Mock<IRepositorioAtividade>();
            repositorioMedicoMoq = new Mock<IRepositorioMedico>();
            ContextoPersistenciaMoq = new Mock<IContextoPersistencia>();
            validadorMoq = new Mock<IValidadorMedico>();
            servicoMedico = new ServicoMedico(repositorioMedicoMoq.Object, ContextoPersistenciaMoq.Object, repositorioAtividadeMoq.Object, validadorMoq.Object);
            medico = new Medico("João", "12345-CC");
        }

        [TestMethod]
        public async Task Deve_inserir_medico_caso_ele_for_valido()
        {
            //arrange
            medico = new Medico("João", "12345-CC");

            //action
            var resultado = await servicoMedico.InserirAsync(medico);

            //assert 
            resultado.Should().BeSuccess();
            repositorioMedicoMoq.Verify(x => x.InserirAsync(medico), Times.Once());
        }

        [TestMethod]
        public async Task Nao_deve_inserir_medico_caso_ele_seja_invalido()
        {
            //arrange
            validadorMoq.Setup(x => x.Validate(It.IsAny<Medico>()))
                .Returns(() =>
                {
                    var resultado = new ValidationResult();
                    resultado.Errors.Add(new ValidationFailure("Nome", "Nome não pode ter menos que 3 caracteres"));
                    return resultado;
                });

            //action
            var resultado = await servicoMedico.InserirAsync(medico);

            //assert             
            resultado.Should().BeFailure();
            repositorioMedicoMoq.Verify(x => x.InserirAsync(medico), Times.Never());
        }

        [TestMethod]
        public async Task Nao_deve_inserir_medico_caso_o_crm_ja_esteja_cadastrado()
        {
            //arrange
            string crm = "12345-CC";
            repositorioMedicoMoq.Setup(x => x.SelecionarPorCrm(crm))
                .Returns(() =>
                {
                    return new Medico("João", "12345-CC");
                });

            //action
            var resultado = await servicoMedico.InserirAsync(medico);

            //assert 
            resultado.Should().BeFailure();
            resultado.Reasons[0].Message.Should().Be($"Esse CRM já está sendo usado por um médico");
            repositorioMedicoMoq.Verify(x => x.InserirAsync(medico), Times.Never());
        }

        [TestMethod]
        public async Task Deve_editar_medico_caso_ele_for_valido()
        {
            //arrange           
            medico = new Medico("João", "12345-CC");

            //action
            var resultado = await servicoMedico.EditarAsync(medico);

            //assert 
            resultado.Should().BeSuccess();
            repositorioMedicoMoq.Verify(x => x.Editar(medico), Times.Once());
        }


        [TestMethod]
        public async Task Nao_deve_editar_medico_caso_ele_seja_invalido()
        {
            //arrange
            validadorMoq.Setup(x => x.Validate(It.IsAny<Medico>()))
                .Returns(() =>
                {
                    var resultado = new ValidationResult();
                    resultado.Errors.Add(new ValidationFailure("Nome", "Nome não pode ter menos que 3 caracteres"));
                    return resultado;
                });

            //action
            var resultado = await servicoMedico.EditarAsync(medico);

            //assert             
            resultado.Should().BeFailure();
            repositorioMedicoMoq.Verify(x => x.Editar(medico), Times.Never());
        }

        [TestMethod]
        public async Task Nao_deve_editar_medico_caso_o_crm_ja_esteja_cadastrado()
        {
            //arrange
            repositorioMedicoMoq.Setup(x => x.SelecionarPorCrm("12345-CC"))
                 .Returns(() =>
                 {
                     return new Medico("Marcelo", "12345-CC");
                 });

            //action
            var resultado = await servicoMedico.EditarAsync(medico);

            //assert 
            resultado.Should().BeFailure();

            repositorioMedicoMoq.Verify(x => x.Editar(medico), Times.Never());
        }

        [TestMethod]
        public async Task Deve_excluir_medico_caso_ele_esteja_cadastrado()
        {
            Guid id = Guid.NewGuid();
            //arrange
            var medico = new Medico(id, "João", "12345-CC");

            repositorioMedicoMoq.Setup(x => x.SelecionarPorId(medico.Id))
               .Returns(() =>
               {
                   return medico;
               });

            repositorioMedicoMoq.Setup(x => x.Existe(medico))
               .Returns(() =>
               {
                   return true;
               });

            //action
            var resultado = await servicoMedico.ExcluirAsync(medico.Id);

            //assert 
            resultado.Should().BeSuccess();
            repositorioMedicoMoq.Verify(x => x.Excluir(medico), Times.Once());
        }

        [TestMethod]
        public async Task Nao_deve_excluir_medico_caso_ele_nao_esteja_cadastrado()
        {
            //arrange

            var medico = new Medico("João", "12345-CC");

            repositorioMedicoMoq.Setup(x => x.Existe(medico))
               .Returns(() =>
               {
                   return false;
               });

            //action
            var resultado = await servicoMedico.ExcluirAsync(medico.Id);

            //assert 
            resultado.Should().BeFailure();
            repositorioMedicoMoq.Verify(x => x.Excluir(medico), Times.Never());
        }

        [TestMethod]
        public async Task Nao_deve_excluir_medico_caso_ele_esteja_relacionada_com_atividade()
        {
            var medico = new Medico("João", "12345-CC");

            repositorioMedicoMoq.Setup(x => x.SelecionarPorId(medico.Id))
               .Returns(() =>
               {
                   return medico;
               });

            repositorioMedicoMoq.Setup(x => x.Existe(medico))
                       .Returns(() =>
                       {
                           return true;
                       });

            repositorioAtividadeMoq.Setup(x => x.SelecionarTodos())
                       .Returns(() =>
                       {
                           var medicos = new List<Medico>();
                           medicos.Add(medico);
                           var atividades = new List<Atividade>();
                           atividades.Add(new Atividade( new DateTime(1555, 5, 20), new TimeSpan(20, 0, 0), new TimeSpan(22, 0, 0), TipoAtividadeEnum.Cirurgia, medicos));
                           return atividades;
                       });

            //action
            var resultado = await servicoMedico.ExcluirAsync(medico.Id);

            //assert 
            resultado.Should().BeFailure();
            resultado.Reasons[0].Message.Should().Be("Este médico está registrado em uma atividade!");
        }
    }
}
