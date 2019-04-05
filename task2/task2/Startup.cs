using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using task2.Models.Services.Contracts;
using task2.Models.Services.Implementations;

namespace task2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _secretApiKey = configuration["apiKey"];
            _apiUrl = configuration["WarsawApiUrl"];
        }

        public IConfiguration Configuration { get; }

        private string _secretApiKey;
        private string _apiUrl;
        private static ApiEventLocalManager staticApiEventManager;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            
            services.AddSingleton<ISyncDate, SimpleSyncDate>();
            services.AddSingleton(provider => new ApiCaller(_secretApiKey,_apiUrl));
            services.AddTransient<BaseApiEventGetter>();
            services.AddTransient<TimePeriodApiEventGetter>();

            services.AddSingleton<IEventGetter>(x=> GetLocalApiManager(x.GetService<BaseApiEventGetter>(),x.GetService<TimePeriodApiEventGetter>(),x.GetService<ISyncDate>()));
            services.AddSingleton<IActionProvider>(x => GetLocalApiManager(x.GetService<BaseApiEventGetter>(), x.GetService<TimePeriodApiEventGetter>(), x.GetService<ISyncDate>()));
            services.AddSingleton<IServiceBlocker, TimeServiceBlocker>();

            services.AddHostedService<EventsGetterBacgroundServiceWrapper>();
        }

        private ApiEventLocalManager GetLocalApiManager(IAsyncEventGetter asyncEventGetter1, IAsyncEventGetter asyncEventGetter2, ISyncDate syncDate)
        {
            if (staticApiEventManager == null)
                staticApiEventManager = new ApiEventLocalManager(asyncEventGetter1, asyncEventGetter2, syncDate);
            return staticApiEventManager;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
