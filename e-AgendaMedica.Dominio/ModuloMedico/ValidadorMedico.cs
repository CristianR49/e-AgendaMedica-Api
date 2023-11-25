using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_AgendaMedica.Dominio.ModuloMedico
{
    public class ValidadorMedico : AbstractValidator<Medico>, IValidadorMedico
    {
        public ValidadorMedico() 
        {
            RuleFor(x => x.Nome)
                .NotNull()
                .MinimumLength(3)
                .NotEmpty();

            RuleFor(x => x.Crm)
                .NotNull()
                .NotEmpty()
                .Matches(@"^\d{5}-[A-Z]{2}$");
        }
    }
}
