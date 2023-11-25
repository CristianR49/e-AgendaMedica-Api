using e_AgendaMedica.Dominio.ModuloMedico;
using eAgendaMedica.TestesIntegracao.Compartilhado;
using FizzWare.NBuilder;
using FluentAssertions;

namespace eAgendaMedica.TestesIntegracao.ModuloMedico
{
    [TestClass]
    public class RepositorioMedicoEmOrmTest : TestesIntegracaoBase
    {
        [TestMethod]
        public void Deve_inserir_medico()
        {
            //arrange
            var medico = Builder<Medico>.CreateNew().Build();

            //action
            RepositorioMedico.Inserir(medico);
            ContextoPersistencia.Gravar();

            //assert
            RepositorioMedico.SelecionarPorId(medico.Id).Should().Be(medico);
        }

        [TestMethod]
        public void Deve_editar_medico()
        {
            //arrange
            var medicoId = Builder<Medico>.CreateNew().Persist().Id;

            var medico = RepositorioMedico.SelecionarPorId(medicoId);
            medico!.Nome = "João";
            medico.Crm = "12345-AB";

            //action
            RepositorioMedico.Editar(medico);
            ContextoPersistencia.Gravar();

            //assert
            RepositorioMedico.SelecionarPorId(medico.Id)
                .Should().Be(medico);
        }
        
        [TestMethod]
        public void Deve_excluir_medico()
        {
            //arrange
            var medicoId = Builder<Medico>.CreateNew().Persist().Id;

            var medico = RepositorioMedico.SelecionarPorId(medicoId);
            //action
            RepositorioMedico.Excluir(medico);
             ContextoPersistencia.Gravar();

            //assert
            RepositorioMedico.SelecionarPorId(medico.Id).Should().BeNull();
        }

        [TestMethod]
        public void Deve_selecionar_todos_medicos()
        {
            //arrange
            var joao = Builder<Medico>.CreateNew()
                .With(x => x.Nome = "João")
                .With(x => x.Crm = "12345-BB")
                .Persist();
            var roberto = Builder<Medico>.CreateNew()
                .With(x => x.Nome = "Roberto")
                .With(x => x.Crm = "12345-AA")
                .Persist();


            //action
            var medicos = RepositorioMedico.SelecionarTodos();

            //assert
            medicos.Should().HaveCount(2);
        }
        [TestMethod]
        public void Deve_selecionar_medico_por_id()
        {
            //arrange
            var medico = Builder<Medico>.CreateNew().Persist();

            //action
            var medicosEncontrada = RepositorioMedico.SelecionarPorId(medico.Id);

            //assert            
            medicosEncontrada.Should().Be(medico);
        }
    }
}
