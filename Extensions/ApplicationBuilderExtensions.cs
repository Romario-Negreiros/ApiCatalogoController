namespace ApiCatalogoController.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseAppCors(this WebApplication app)
        {
            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            });

            return app;
        }

        public static WebApplication UseOpenApi(this WebApplication app)
        {
            IWebHostEnvironment env = app.Environment;
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            return app;
        }
    }
}
