using e_AgendaMedica.Dominio.Compartilhado;
using e_AgendaMedica.Dominio.ModuloMedico;
using e_AgendaMedica.Infra.Orm.Compartilhado;
using e_AgendaMedica.Infra.Orm.ModuloMedico;
using eAgendaMedica.Aplicacao.ModuloMedico;
using eAgendaMedica.WebApi.Config.Profiles;
using Microsoft.EntityFrameworkCore;

namespace eAgendaMedica.WebApi
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("SqlServer");

            builder.Services.AddDbContext<IContextoPersistencia, eAgendaMedicaDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(connectionString);
            });

            builder.Services.AddTransient<IRepositorioMedico, RepositorioMedicoOrm>();
            builder.Services.AddTransient<ServicoMedico>();

            builder.Services.AddAutoMapper(config =>
            {
                config.AddProfile<MedicoProfile>();
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}