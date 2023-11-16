using FluentValidation;

namespace e_AgendaMedica.Dominio.ModuloAtividade
{
    public class ValidadorAtividade : AbstractValidator<Atividade>
    {
        public ValidadorAtividade()
        {
            RuleFor(x => x.Data)
                .NotNull().NotEmpty();

            RuleFor(x => x.HorarioInicio)
                .NotNull().NotEmpty();

            RuleFor(x => x.HorarioTermino)
                .NotNull().NotEmpty();

            RuleFor(x => x.TipoAtividade)
                .NotNull()
                .NotEmpty()
                .IsInEnum();

            RuleFor(x => x.Medicos)
                .NotEmpty().Must(x => x.Count >= 1)
                .WithMessage("No mínimo um médico precisa ser informado");
        }
    }
}
