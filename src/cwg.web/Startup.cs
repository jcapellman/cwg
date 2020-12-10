using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;   
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using cwg.web.Data;
using cwg.web.Repositories;
using cwg.web.Services;
using NJsonSchema.Generation;

namespace cwg.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<ClipboardService>();

            services.AddSingleton(new GeneratorRepository());
            services.AddSingleton<GeneratorsService>();

            services.AddControllers();

            services.AddSwaggerDocument(a =>
            {
                a.Description = "Swagger documentation for the cwg services";
                a.Version = "1.0.1";
                a.Title = "cwg";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseDeveloperExceptionPage();
           
            app.UseStaticFiles();

            app.UseOpenApi(configure => configure.PostProcess = (document, _) 
                => document.Schemes = new[] { NSwag.OpenApiSchema.Https });

            app.UseSwaggerUi3(a =>
            {
                a.DocumentTitle = "cwg";
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}