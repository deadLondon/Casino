using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CasinoMvc.Data;
namespace CasinoMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<CasinoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CasinoContext") ?? throw new InvalidOperationException("Connection string 'CasinoContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHostedService<CasinoService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Player}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
