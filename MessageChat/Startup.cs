using System;
using MessageChat.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MessageChat.DataRepositories;
using MessageChat.AuthorizedAccountRepository;
using Microsoft.Extensions.Configuration;

namespace MessageChat
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
            services.Configure<ConnectionSetting>(Configuration.GetSection("ConnectionStrings"));
            services.AddSingleton<MessageHub>();
            services.AddSingleton<IAuthorizedUsersRepository, AuthorizedUsers>();
            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IMessageRepository, MessageRepository>();
            services.AddSingleton<DbHelper>();
            services.AddControllers().AddControllersAsServices();
            services.AddCors();
            services.AddSignalR(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromMinutes(5);
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
            });
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:3000");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/lothub");
            });
        }
    }
}