using e_AgendaMedica.Dominio.ModuloAtividade;
using e_AgendaMedica.Dominio.ModuloMedico;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgendaMedica.TestesUnitarios.Dominio.ModuloAtividade
{
    [TestClass]
    public class ValidadorAtividadeTest
    {
        private Atividade Atividade { get; set; }
        private ValidadorAtividade Validador { get; set; }

        public ValidadorAtividadeTest()
        {
            List<Medico> medicos = new List<Medico>();

            Atividade = new Atividade();

            Atividade.Medicos = medicos;

            Validador = new ValidadorAtividade();
        }

        [TestMethod]
        public void Data_atividade_deve_ser_valido()
        {
            //arrange
            Atividade.Data = DateTime.Parse("10/2/2003");

            //action
            var resultado = Validador.TestValidate(Atividade);

            //assert
            resultado.ShouldNotHaveValidationErrorFor(x => x.Data);
        }

        [TestMethod]
        public void DataConclusao_atividade_deve_ser_valido()
        {
            //arrange
            Atividade.DataConclusao = DateTime.Parse("10/2/2003");

            //action
            var resultado = Validador.TestValidate(Atividade);

            //assert
            resultado.ShouldNotHaveValidationErrorFor(x => x.DataConclusao);
        }

        [TestMethod]
        public void HorarioInicio_atividade_deve_ser_valido()
        {
            //arrange
            Atividade.HorarioInicio = TimeSpan.Parse("14:55");

            //action
            var resultado = Validador.TestValidate(Atividade);

            //assert
            resultado.ShouldNotHaveValidationErrorFor(x => x.HorarioInicio);
        }

        [TestMethod]
        public void HorarioTermino_atividade_deve_ser_valido()
        {
            //arrange
            Atividade.HorarioTermino = TimeSpan.Parse("14:59");

            //action
            var resultado = Validador.TestValidate(Atividade);

            //assert
            resultado.ShouldNotHaveValidationErrorFor(x => x.HorarioTermino);
        }

        [TestMethod]
        public void TipoAtividade_atividade_deve_ser_valido()
        {
            //arrange
            Atividade.TipoAtividade = TipoAtividadeEnum.Cirurgia;

            //action
            var resultado = Validador.TestValidate(Atividade);

            //assert
            resultado.ShouldNotHaveValidationErrorFor(x => x.TipoAtividade);
        }

        [TestMethod]
        public void Medicos_atividade_deve_pelo_menos_um()
        {
            //arrange

            List<Medico> medicos = new List<Medico>();

            medicos.Add(new Medico("João", "12345-AA"));

            Atividade.Medicos = medicos;

            //action
            var resultado = Validador.TestValidate(Atividade);

            //assert
            resultado.ShouldNotHaveValidationErrorFor(x => x.Medicos);
        }
    }
}
