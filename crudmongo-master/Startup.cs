using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace aspnetdemo2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));
            services.Configure<EstudiantesDatabaseSettings>(

                Configuration.GetSection("EstudiantesDatabaseSettings")
            );

            services.AddAuthentication()
                .AddCookie(options => {
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                });

            services.AddSingleton<IEstudiantesDatabaseSettings>(
                sp => 
                sp.GetRequiredService<IOptions<EstudiantesDatabaseSettings>>().Value
            );
            // services.AddTransient<IEstudiantesRespository, EstudiantesRespository>();
            services.AddSingleton<IEstudiantesRespository, MongoEstudiantesRespository>();
            services.AddControllers();
            var mvc =services.AddRazorPages();
              services.AddMvc(setup => {
      //...mvc setup...
                })
                .AddFluentValidation(fl => 
                fl.RegisterValidatorsFromAssembly((typeof(Startup)).Assembly)
                );

            services.AddSwaggerGen();
            //services.AddFluentValidation
           // mvc.AddFluentValidation();
            //services.AddFluentValidation()  ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env
            , UserManager<IdentityUser> userManager
            , RoleManager<IdentityRole> roleManager
        )
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
                
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
            try{
                SeedIdentityUsers.SeedData(userManager, roleManager);
            }catch(Exception ex){
                System.Console.WriteLine("Error al iniciar usuarios", ex.Message);
            }
        }
    }
}
