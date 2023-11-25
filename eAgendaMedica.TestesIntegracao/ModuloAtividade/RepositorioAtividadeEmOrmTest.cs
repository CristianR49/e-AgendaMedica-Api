using e_AgendaMedica.Dominio.ModuloAtividade;
using eAgendaMedica.TestesIntegracao.Compartilhado;
using FizzWare.NBuilder;
using FluentAssertions;

namespace eAgendaMedica.TestesIntegracao.ModuloAtividade
{
    [TestClass]
    public class RepositorioAtividadeEmOrmTest : TestesIntegracaoBase
    {
        [TestMethod]
        public void Deve_inserir_atividade()
        {
            //arrange
            var atividade = Builder<Atividade>.CreateNew().Build();

            //action
            RepositorioAtividade.Inserir(atividade);
            ContextoPersistencia.Gravar();

            //assert
            RepositorioAtividade.SelecionarPorId(atividade.Id).Should().Be(atividade);
        }

        [TestMethod]
        public void Deve_editar_atividade()
        {
            //arrange
            var atividadeId = Builder<Atividade>.CreateNew().Persist().Id;

            var atividade = RepositorioAtividade.SelecionarPorId(atividadeId);

            //action
            RepositorioAtividade.Editar(atividade);
            ContextoPersistencia.Gravar();

            //assert
            RepositorioAtividade.SelecionarPorId(atividade.Id)
                .Should().Be(atividade);
        }

        [TestMethod]
        public void Deve_excluir_atividade()
        {
            //arrange
            var atividadeId = Builder<Atividade>.CreateNew().Persist().Id;

            var atividade = RepositorioAtividade.SelecionarPorId(atividadeId);
            //action
            RepositorioAtividade.Excluir(atividade);
            ContextoPersistencia.Gravar();

            //assert
            RepositorioAtividade.SelecionarPorId(atividade.Id).Should().BeNull();
        }

        [TestMethod]
        public void Deve_selecionar_todos_atividades()
        {
            //arrange
            var joao = Builder<Atividade>.CreateNew().Persist();
            var roberto = Builder<Atividade>.CreateNew().Persist();


            //action
            var atividades = RepositorioAtividade.SelecionarTodos();

            //assert
            atividades.Should().HaveCount(2);
        }
        [TestMethod]
        public void Deve_selecionar_atividade_por_id()
        {
            //arrange
            var atividade = Builder<Atividade>.CreateNew().Persist();

            //action
            var atividadesEncontrada = RepositorioAtividade.SelecionarPorId(atividade.Id);

            //assert            
            atividadesEncontrada.Should().Be(atividade);
        }

    }
}
