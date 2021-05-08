using DemoEFTriggerProject.DBContext;
using DemoEFTriggerProject.Models;
using DemoEFTriggerProject.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace DemoEFTriggerProject
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
            services.AddControllers();

            services.Configure<WebEncoderOptions>(options =>
                        options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

            services.AddHttpContextAccessor();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseTriggers(triggerOptions => triggerOptions.AddAssemblyTriggers());
                options.UseInMemoryDatabase("test");
            });

            var tokenOptionSection = Configuration.GetSection("TokenOptions");
            services.Configure<TokenOptionModel>(tokenOptionSection);
            services.AddCustomJwtService(tokenOptionSection.Get<TokenOptionModel>());

            services.AddScoped<DemoService>();

            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DemoEFTriggerProject", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoEFTriggerProject v1"));
            }

            app.UseStaticHttpContext();

            app.UseCors(opts =>
                           opts.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                       );

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
