using e_AgendaMedica.Dominio.ModuloMedico;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgendaMedica.TestesUnitarios.Dominio.ModuloMedico
{
    [TestClass]
    public class ValidadorMedicoTest
    {
        private Medico Medico { get; set; }
        private ValidadorMedico Validador { get; set; }

        public ValidadorMedicoTest()
        {
            Medico = new Medico();

            Validador = new ValidadorMedico();
        }

        [TestMethod]
        public void Nome_medico_deve_ter_no_minimo_3_caracteres()
        {
            //arrange
            Medico.Nome = "abcd";

            //action
            var resultado = Validador.TestValidate(Medico);

            //assert
            resultado.ShouldNotHaveValidationErrorFor(x => x.Nome);
        }

        [TestMethod]
        public void Crm_medico_deve_seguir_o_regex()
        {
            //arrange
            Medico.Crm = "12345-AB";

            //action
            var resultado = Validador.TestValidate(Medico);

            //assert
            resultado.ShouldNotHaveValidationErrorFor(x => x.Crm);
        }
    }
}
