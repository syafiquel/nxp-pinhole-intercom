using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using IntercomCameraWebApi.Services;
using IntercomCameraWebApi.Data;

namespace IntercomCameraWebApi
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }

        public Startup()
        {
            var builder = new ConfigurationBuilder();
            Configuration = builder.Build();   
        }    

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IHttpClientService, HttpClientService>();
            services.AddDbContext<WebApiDbContext>( options => {
                options.UseNpgsql("Server=db;Port=5432;Database=trx;User ID=postgres;Password=postgres");
                }
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            var wsOptions = new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(120) };
            app.UseWebSockets(wsOptions);

            // app.Run(async (context) =>  {
            //     if(context.Request.Path == "/ws")    {
                    
            //     } else {
            //         context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            //     }
            // });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private async Task Send(HttpContext context, WebSocket webSocket) {
            await Task.Run(() => log());
        }

        private void log() {
            Console.WriteLine("ws con");
        }
    }

}
