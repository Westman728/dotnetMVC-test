using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StockAnalysis.Core.Repositories;
using StockAnalysis.Core.Services;

namespace StockAnalysis.Web
{
    public static class WebAppInitializer
    {
        public static WebApplicationBuilder CreateBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ISignalRepository, SignalRepository>();
            builder.Services.AddScoped<ISignalService, SignalService>();

            builder.Services.AddSingleton<IModelIntegrationService>(provider =>
                new ModelIntegrationService(
                    builder.Configuration["PythonSettings:PythonPath"],
                    builder.Configuration["PythonSettings:ModelScriptPath"]
                ));

            return builder;
        }

        public static void ConfigureApp(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}

