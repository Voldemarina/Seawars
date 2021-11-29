using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Seawars.Interfaces.Services;
using Seawars.WebApi.Clients.Connection;
using Seawars.WebApi.Clients.Field;
using Seawars.WebApi.Clients.Game;

namespace Seawars.WebApi
{
    public record Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IConnection, GameConnection>();
            services.AddScoped<IUserField, UserField>();
            services.AddScoped<IBattleGround, BattleGround>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
