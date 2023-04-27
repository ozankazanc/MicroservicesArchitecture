using FreeCourse.Services.Basket.Services;
using FreeCourse.Services.Basket.Settings;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket
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
            //Authenticate olmuþ bir user zorunluluðu getiriliyor. AddController içerisinde ise yürütülmesini saðlýyoruz.
            var requiredAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            /// <summary>
            ///DefaultInboundClaimTypeMap ne iþe yarar?
            ///sub içerisinde servise istek yapan kullanýcýnýn id'si bulunur. Framework bizim belirlediðimiz "sub"ý namedIndentifier isimli bir claim'e dönüþtürür.
            ///Dönüþtürme iþlemini yapmamasý için aþaðýdaki þekilde konfigure ediyoruz.
            ///config öncesi gelen deðer: {http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier: fb69010b-0c08-4d1e-9ebc-b8350585dcaa}
            ///config sonrasý gelen deðer. {sub: fb69010b-0c08-4d1e-9ebc-b8350585dcaa}
            /// </summary>
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            //Microservisi koruma altýna alma iþlemi.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"];
                options.Audience = "resource_basket";
                options.RequireHttpsMetadata = false;
            });

            //Shared için eklendi. HttpContext nesnesi shared projesine aktarýlacak.
            services.AddHttpContextAccessor();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            /////////////////
            
            //Eklendi.
            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));
            //
            services.AddSingleton<RedisService>(sp =>
            {
                var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
                var redis = new RedisService(redisSettings.Host, redisSettings.Port);
                redis.Connect();
                return redis;
            });

            services.AddControllers(opt => 
            {
                opt.Filters.Add(new AuthorizeFilter(requiredAuthorizePolicy));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Basket", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Basket v1"));
            }

            app.UseRouting();

            //Eklendi.
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
