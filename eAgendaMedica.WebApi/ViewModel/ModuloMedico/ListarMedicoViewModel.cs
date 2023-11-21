namespace eAgendaMedica.WebApi.ViewModel.ModuloMedico
{
    public partial class MedicoViewModel
    {
        public class ListarMedicoViewModel
        {
            public Guid Id { get; set; }
            public string Nome { get; set; }
            public string Crm { get; set; }
        }
    }
}
