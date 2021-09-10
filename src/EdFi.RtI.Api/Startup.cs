using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EdFi.RtI.Core;
using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.KeyVault;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.UserRoleMappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EdFi.RtI.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            services.AddCoreLayer();

            RegisterAppSettings(services);

            // If using Kestrel
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            
            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddHttpContextAccessor();

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvc(options =>
            {
                //options.Filters.Add<AllowAnonymousFilter>(); // TODO Uncomment this to enable anonymous. Comment to disable.
            });

            ConfigureAuthentication(services);
        }

        private void RegisterAppSettings(IServiceCollection services)
        {
            var appSettings = new AppSettings { StartupMode = Configuration["StartupMode"] };
            services.AddSingleton(appSettings);

            var cacheAppSettings = Configuration.GetSection("Cache").Get<CacheAppSettings>();
            services.AddSingleton(cacheAppSettings);

            var keyVaultAppSettings = Configuration.GetSection("KeyVault").Get<KeyVaultAppSettings>();
            services.AddSingleton(keyVaultAppSettings);

            var userRoleMappingsAppSettings = Configuration.GetSection("UserRoleMappings").Get<UserRoleMappingsAppSettings>();
            services.AddSingleton(userRoleMappingsAppSettings);
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = Configuration["Authentication:Authority"];
                options.Audience = Configuration["Authentication:Audience"];
                options.IncludeErrorDetails = true;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    NameClaimType = ClaimTypes.NameIdentifier
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        if (context.SecurityToken is JwtSecurityToken token)
                        {
                            string email = token.Claims.FirstOrDefault(claim => claim.Type.Contains("preferred_username"))?.Value;

                            if (context.Principal.Identity is ClaimsIdentity identity)
                            {
                                identity.AddClaim(new Claim("access_token", token.RawData));
                                identity.AddClaim(new Claim("email", email));
                            }
                        }

                        return Task.FromResult(0);
                    },
                    OnAuthenticationFailed = AuthenticationFailed
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.Name);
                });
            });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme);

                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
        }

        private Task AuthenticationFailed(AuthenticationFailedContext arg)
        {
            // For debugging purposes only!
            var s = $"AuthenticationFailed: {arg.Exception.Message}";

            arg.Response.ContentLength = s.Length;
            arg.Response.Body.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);

            return Task.FromResult(0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers()
                    .RequireAuthorization();
            });
        }
    }

    public static class StartupExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app) => app.UseExceptionHandler(options =>
        {
            options.Run(async context =>
            {
                var interceptedError = context.Features.Get<IExceptionHandlerFeature>();

                if (interceptedError == null || interceptedError.Error == null)
                    return;

                var response = HttpErrorResponse.FromException(interceptedError.Error);
                context.Response.StatusCode = response.StatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response.ToString(), Encoding.UTF8);
            });
        });
    }
}
