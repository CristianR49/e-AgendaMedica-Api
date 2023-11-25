using FluentValidation.Results;

namespace e_AgendaMedica.Dominio.Compartilhado
{
    public interface IValidador<T> where T : Entidade<T>
    {
        public ValidationResult Validate(T instance);
    }
}
