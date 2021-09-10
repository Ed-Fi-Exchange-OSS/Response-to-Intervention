using EdFi.RtI.Api.Configuration;
using EdGraph.Common.Core.MVC.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EdFi.RtI.Api
{
    /// <summary>
    /// Startup class used by Swashbuckle command line tool as an entry point to generate Swagger file in CI/CD
    /// </summary>
    public class SwaggerStartup
    {
        private readonly IConfiguration configuration;

        public SwaggerStartup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var servicesOptions = this.configuration.GetSection("Services").Get<ServicesOptions>();

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });

            //services.AddApiVersioning(config =>
            //{
            //    config.DefaultApiVersion = new ApiVersion(1, 0);
            //    config.AssumeDefaultVersionWhenUnspecified = true;
            //    config.ReportApiVersions = true;
            //    config.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"), new HeaderApiVersionReader("X-version"));
            //});

            //services.AddVersionedApiExplorer(
            //    options =>
            //    {
            //        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            //        // note: the specified format code will format the version as "'v'major[.minor][-status]"
            //        options.GroupNameFormat = "'v'VVV";
            //    });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Response to Intervention API",
                    Version = "v1",
                    Description = "The Management HTTP API.",
                    TermsOfService = new Uri("https://edgraph.com/legal/termsofservices"),
                    Contact = new OpenApiContact
                    {
                        Name = "EdGraph Support Team",
                        Email = "support@edgraph.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under EdGraph license",
                        Url = new Uri("https://edgraph.com/legal/license"),
                    }
                });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{servicesOptions.IdentityWebExternal}/connect/authorize"),
                            TokenUrl = new Uri($"{servicesOptions.IdentityWebExternal}/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "https://api.edgraph.com/auth/management", "Management API" }
                            }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
                options.CustomSchemaIds(type => DefaultSchemaIdSelector(type));

                var filePath = Path.Combine(AppContext.BaseDirectory, "EdFi.RtI.Api.xml");
                options.IncludeXmlComments(filePath);
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
        }

        private string DefaultSchemaIdSelector(Type modelType)
        {
            if (!modelType.IsConstructedGenericType) return modelType.ToString();

            var prefix = modelType.GetGenericArguments()
                .Select(genericArg => DefaultSchemaIdSelector(genericArg))
                .Aggregate((previous, current) => previous + current);

            return prefix + modelType.Name.Split('`').First();
        }
    }
}