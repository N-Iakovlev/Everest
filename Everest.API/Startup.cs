namespace Everest.API
{
    #region << Using >>

    using Everest.Domain;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Incoding.Core;
    using Incoding.Core.Block.IoC;
    using Incoding.Core.Block.IoC.Provider;
    using Incoding.Core.Block.Logging;
    using Incoding.Core.Block.Logging.Core;
    using Incoding.Core.Block.Logging.Loggers;
    using Incoding.Core.Extensions;
    using Incoding.Data.EF;
    using Incoding.Web;
    using Incoding.Web.MvcContrib;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Net.Http.Headers;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;
    using NUglify.Css;
    using NUglify.JavaScript;
    using CompressionLevel = System.IO.Compression.CompressionLevel;
    using SameSiteMode = SameSiteMode;

    #endregion

    public class Startup
    {
        private readonly IConfiguration configuration;

        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = this.configuration.GetConnectionString("Main");
            var smtpSettings = configuration.GetSection("SmtpSettings").Get<MailSettings>();
            LoggingFactory.Instance.Initialize(logging =>
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (this.env.IsDevelopment())
                {
                    logging.WithPolicy(policy => policy.For(LogType.Debug).Use(new ConsoleLogger()));
                    logging.WithPolicy(policy => policy.For(LogType.Trace).Use(new ConsoleLogger()));
                }
                else
                {
                    logging.WithPolicy(policy => policy.For(LogType.Debug).Use(FileLogger.WithAtOnceReplace(path, () => "Debug_{0}.txt".F(DateTime.Now.ToString("yyyyMMdd")))));
                    logging.WithPolicy(policy => policy.For(LogType.Trace).Use(FileLogger.WithoutReplace(path, () => "Trace_{0}.txt".F(DateTime.Now.ToString("yyyyMMdd")))));
                }
            });
            services.AddSingleton<EmailService>(provider => new EmailService(smtpSettings));
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.SameAsRequest;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy.
                options.FallbackPolicy = options.DefaultPolicy;
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddHttpContextAccessor();

            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Clear();

                options.ModelBinderProviders.Add(new FormFileModelBinderProvider());
                options.ModelBinderProviders.Add(new FormCollectionModelBinderProvider());
                options.ModelBinderProviders.Add(new ComplexObjectModelBinderProvider());
                options.ModelBinderProviders.Add(new SimpleTypeModelBinderProvider());
                options.ModelBinderProviders.Add(new ArrayModelBinderProvider());
                options.ModelBinderProviders.Add(new CollectionModelBinderProvider());
                options.ModelBinderProviders.Add(new DictionaryModelBinderProvider());
                options.ModelBinderProviders.Add(new FloatingPointTypeModelBinderProvider());
            })
                    .AddFluentValidation(fluentValidation =>
                    {
                        fluentValidation.ImplicitlyValidateChildProperties = true;
                        fluentValidation.RunDefaultMvcValidationAfterFluentValidationExecutes = false;

                        fluentValidation.ValidatorFactory = new IncValidatorFactory();

                        AssemblyScanner.FindValidatorsInAssembly(typeof(AddOrEditEmployeeCommand).Assembly).ForEach(r =>
                        {
                            services.Add(ServiceDescriptor.Transient(r.InterfaceType, r.ValidatorType));
                            services.Add(ServiceDescriptor.Transient(r.ValidatorType, r.ValidatorType));
                        });
                    });

            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddJavaScriptBundle("/js/libs.js",
                                             CodeSettings.Pretty(),
                                             "/lib/incoding-framework/dist/js/bootstrap.js",
                                             "/lib/incoding-framework/dist/js/adminlte.min.js",
                                             "/lib/incoding-framework/dist/js/incoding.framework.js");

                pipeline.AddCssBundle("/css/styles.css",
                                      CssSettings.Pretty(),
                                      "/css/*.css");
            },
                                     options =>
                                     {
                                         options.HttpsCompression = HttpsCompressionMode.Compress;

                                         if (!this.env.IsDevelopment())
                                         {
                                             options.EnableTagHelperBundling = true;

                                             options.EnableCaching = true;
                                             options.EnableMemoryCache = true;
                                         }
                                     });

            services.ConfigureIncodingCoreServices();
            services.ConfigureIncodingIMLServices();
            services.ConfigureIncodingWebServices();
            services.ConfigureIncodingNhDataServices(typeof(EverestEntityBase),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"ConnectionsString{connectionString.GetHashCode()}.cfg"),
                fluentConfiguration =>
                {
                    return Fluently.Configure(new Configuration())
                        .Database(PostgreSQLConfiguration.Standard.ConnectionString(connectionString))
                        .ExposeConfiguration(cfg =>
                        {
                            new SchemaUpdate(cfg).Execute(false, true);
                            cfg.SetProperty("command_timeout", "100")
                                .SetProperty("connection_timeout", "0")
                                .SetProperty("generate_statistics", "true");
                        })
                        .Mappings(cfg => cfg.FluentMappings.AddFromAssembly(typeof(EverestEntityBase).Assembly));
                });

            services.AddDataProtection()
                    .SetApplicationName($"cpb-ip-{this.env.EnvironmentName}")
                    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(this.env.ContentRootPath, "keys")));


            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();

            services.AddMvc(options => options.MaxModelBindingCollectionSize = int.MaxValue);

            services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; })
                    .Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.SmallestSize; })
                    .AddResponseCompression(options =>
                    {
                        options.EnableForHttps = true;
                        options.Providers.Add<BrotliCompressionProvider>();
                        options.Providers.Add<GzipCompressionProvider>();
                        options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
                    });
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime applicationLifetime, IWebHostEnvironment env)
        {
            IoCFactory.Instance.Initialize(init => init.WithProvider(new MSDependencyInjectionIoCProvider(app.ApplicationServices)));

            app.Use(async (ctx, next) => await next.Invoke());

            app.UseExceptionHandler(builder => builder.Run(async ctx =>
            {
                var exception = ctx.Features.Get<IExceptionHandlerPathFeature>();
                LoggingFactory.Instance.LogException(LogType.Debug, exception.Error);
                await ctx.Response.WriteAsync(new DefaultParserException().Parse(exception.Error));
            }));

            app.UseRouting();

            //app.UseHsts();
            //app.UseHttpsRedirection();

            app.UseWebOptimizer();
            app.UseStaticFiles(new StaticFileOptions
            {
                HttpsCompression = HttpsCompressionMode.Compress,
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(30)
                    };
                }
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode == 401)
                {
                    if (context.HttpContext.Request.IsAjaxRequest())
                    {
                        response.StatusCode = 200;
                        await response.WriteAsync(IncodingResult.RedirectTo("/Account/Login").ToString());
                    }
                }
            });

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Main}/{id?}");
                foreach (var name in new[]
                         {
                             "Employee",
                             "Product",
                             "Order",
                             "Contacts",
                             "Category",
                             "Content",
                             "Cart"
                         })
                {
                    endpoints.MapControllerRoute(name, name, new { controller = "Home", action = "Main", incView = $"~/Views/App/{name}/Index.cshtml" });
                    endpoints.MapControllerRoute(name, name, new { controller = "Home", action = "Admin", incView = $"~/Views/Admin/{name}/Index.cshtml" });
                }
            



                //endpoints.MapControllers().RequireAuthorization();
            });

            app.UseResponseCompression();

        }
    }

    static class StartupExtensions
    {
        public static IServiceCollection ConfigureIncodingIMLServices(this IServiceCollection services)
        {
            services.ConfigureJqueryAjaxOptions(ajax => { ajax.Cache = false; })
                    .ConfigureJqueryAjaxFormOptions(formAjax => { formAjax.Type = JqueryAjaxOptions.HttpVerbs.Post; });

            IncodingHtmlHelper.Def_Label_Class = B.Col_xs_3;
            IncodingHtmlHelper.Def_Control_Class = B.Col_xs_6;
            IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v3;

            TemplateHandlebarsFactory.GetVersion = () => Guid.NewGuid().ToString();
            IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v3;

            return services;
        }

        private static IServiceCollection ConfigureJqueryAjaxOptions(this IServiceCollection services, Action<JqueryAjaxOptions> options)
        {
            options(JqueryAjaxOptions.Default);

            return services;
        }

        private static IServiceCollection ConfigureJqueryAjaxFormOptions(this IServiceCollection services, Action<JqueryAjaxFormOptions> options)
        {
            options(JqueryAjaxFormOptions.Default);

            return services;
        }
    }
}