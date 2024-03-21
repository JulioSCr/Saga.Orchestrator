using Microsoft.OpenApi.Models;

namespace Saga.Orchestrator.API.Configuration
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Saga Orchestrator API",
                    Description = "Developed for testing Saga Pattern",
                    Contact = new OpenApiContact() { Name = "Julio Cruz", Email = "julioscruzny@gmail.com" }
                });

                GetXmlFiles().ForEach(xml => options.IncludeXmlComments(xml));
            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }

        private static List<string> GetXmlFiles()
        {
            return new List<string>
            {
                GetXmlFile("Saga.Orchestrator.API.xml")
            };
        }

        private static string GetXmlFile(string xmlFileName) => Path.Combine(AppContext.BaseDirectory, xmlFileName);
    }
}
