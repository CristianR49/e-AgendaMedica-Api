﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_AgendaMedica.Dominio.ModuloMedico
{
    public class ValidadorMedico : AbstractValidator<Medico>
    {
        public ValidadorMedico() 
        {
            RuleFor(x => x.Crm)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(8)
                .Matches(@"/^\d{5}-[A-Z]{2}$/");
        }
    }
}
