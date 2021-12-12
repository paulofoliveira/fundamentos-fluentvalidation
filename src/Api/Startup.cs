using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(options =>  // Integração do FluentValidation com o ASP.NET Core Pipeline
                {
                    options.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();
                });

            // services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>(); Registra manualmente para cada Request seu respectivo Validator

            services.AddTransient<StudentRepository>();
            services.AddTransient<CourseRepository>();


            // ValidatorOptions.Global.CascadeMode = CascadeMode.Stop; Ativar CascadeMode a nível de aplicação.
            // CascadeMode controla o flow de validação. Pode ser aplicado a nível global (como acima), a nível de Validator ou propriedade.


        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
