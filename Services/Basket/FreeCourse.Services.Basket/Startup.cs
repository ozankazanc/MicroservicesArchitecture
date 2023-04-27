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
            //Authenticate olmu� bir user zorunlulu�u getiriliyor. AddController i�erisinde ise y�r�t�lmesini sa�l�yoruz.
            var requiredAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            /// <summary>
            ///DefaultInboundClaimTypeMap ne i�e yarar?
            ///sub i�erisinde servise istek yapan kullan�c�n�n id'si bulunur. Framework bizim belirledi�imiz "sub"� namedIndentifier isimli bir claim'e d�n��t�r�r.
            ///D�n��t�rme i�lemini yapmamas� i�in a�a��daki �ekilde konfigure ediyoruz.
            ///config �ncesi gelen de�er: {http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier: fb69010b-0c08-4d1e-9ebc-b8350585dcaa}
            ///config sonras� gelen de�er. {sub: fb69010b-0c08-4d1e-9ebc-b8350585dcaa}
            /// </summary>
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            //Microservisi koruma alt�na alma i�lemi.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"];
                options.Audience = "resource_basket";
                options.RequireHttpsMetadata = false;
            });

            //Shared i�in eklendi. HttpContext nesnesi shared projesine aktar�lacak.
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
