using FreeCourse.Services.Basket.Consumers;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Services.Basket.Settings;
using FreeCourse.Shared.Services;
using MassTransit;
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
            services.AddMassTransit(x =>
            {
                x.AddConsumer<BasketCourseNameChangedEventConsumer>();

                //https://hub.docker.com/_/rabbitmq
                //rabbitMQ default port :5672
                //rabbitMQ gelen mesajları görebileceğişmiz uı :15672
                x.UsingRabbitMq((context, cnfg) =>
                {
                    cnfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                    cnfg.ReceiveEndpoint("course-name-change-event-basket-service", e =>
                    {
                        e.ConfigureConsumer<BasketCourseNameChangedEventConsumer>(context);
                    });

                });
            });
            services.AddMassTransitHostedService();
            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");//user.claim ile ya da httpContext.Contex.Us4er.Claim ile gelen kullan?c? datalar? jwt nin payload?ndaki keyleri ba?ka bir de?i?ken ismine maplemesin diye bu i?lemi yapt?k.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.Authority = Configuration["IdentityServerUrl"];//bu microservice e token ? kimin da?itt??? bilgisini veriyoruz 
                opt.Audience = "resource_basket";//En:Audience => Tr:?zleyici(gelen token içindeki audience parametresine bak?cak resource_token bilgisi varsa, tamam sen istek yapabilirsin buraya deyip iizn vericek)
                opt.RequireHttpsMetadata = false;//http protokülü olarak https bekler defaultta o yüzden false verdik
                

            });
            services.AddHttpContextAccessor(); //Shared klasöründe userId sini alabilmek için yazd???m?z serviste IhhtpContextaccessor  inteface inikulland?k. bu interface in dolu gelm4esi için buraya ekledik.User ?d yi framework gelen jwt içnden context üzerinden okuyabiliyoruz. user id("sub" key inin de?eri) de?erini almak ?için de GetuserId() metotunu SharedIdentityService in içinde GEtUserId() isminde olu?turduk
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<IBasketService, BasketService>();
            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));
            services.AddSingleton<RedisService>(sp =>
            {
                var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
                var redis = new RedisService(redisSettings.Host, redisSettings.Port);
                redis.Connect();
                return redis;
            });
            services.AddControllers(att => att.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy))); 
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
