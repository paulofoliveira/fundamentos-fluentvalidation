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
                .AddFluentValidation(options =>  // Integra��o do FluentValidation com o ASP.NET Core Pipeline
                {
                    options.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();
                });

            // services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>(); Registra manualmente para cada Request seu respectivo Validator

            services.AddTransient<StudentRepository>();
            services.AddTransient<CourseRepository>();


            // ValidatorOptions.Global.CascadeMode = CascadeMode.Stop; Ativar CascadeMode a n�vel de aplica��o.
            // CascadeMode controla o flow de valida��o. Pode ser aplicado a n�vel global (como acima), a n�vel de Validator ou propriedade.


        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
