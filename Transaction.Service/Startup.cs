using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCouch;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Tradeas.Colfinancial.Provider;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Colfinancial.Provider.Repositories;

namespace Tradeas.Service.Api
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
            services.AddMvc()
                    .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ");

            services.AddTransient<IWebDriver>(factory => {
                var options = new ChromeOptions();
                options.AddArgument("--headless");
                var chromeDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return new ChromeDriver(chromeDriverPath, options);
            });
            services.AddTransient<IMyCouchClient>(factory =>
            {
                return new MyCouchClient("http://127.0.0.1:5984", "transactions");
            });

            services.AddTransient<IBuilder, TransactionBuilder>();
            services.AddTransient<IDatabaseProcessor, DatabaseProcessor>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IExtractor, Extractor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
