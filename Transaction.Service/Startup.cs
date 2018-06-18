using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Tradeas.Colfinancial.Provider;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Colfinancial.Provider.Scrapers;
using Tradeas.Repositories;
using Tradeas.Service.Api.Builders;
using Tradeas.Service.Api.Processors;

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

            var couchdbUrl = "http://127.0.0.1:5984";
            services
                .AddTransient<IWebDriver>(factory =>
                {
                    var options = new ChromeOptions();
                    //options.AddArgument("--headless");
                    var chromeDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    return new ChromeDriver(chromeDriverPath, options);
                })
                .AddTransient<IJournalBuilder, JournalBuilder>()
                .AddTransient<ITransactionScraper, TradeTransactionScraper>()
                .AddTransient<IPortfolioScraper, PortfolioScraper>()
                .AddTransient<IJournalProcessor, JournalProcessor>()
                .AddTransient<IJournalStageProcessor, JournalStageProcessor>()
                .AddTransient<ITransactionBuilder, TransactionBuilder>()
                .AddTransient<ITransactionProcessor, TransactionProcessor>()
                .AddTransient(typeof(BrokerTransactionScraper))
                .AddTransient(typeof(BrokerTransactionBuilder))
                .AddTransient(typeof(BrokerTransactionProcessor))
                .AddTransient<IJournalRepository>(factory => new JournalRepository(couchdbUrl))
                .AddTransient<ITransactionRepository>(factory => new TransactionRepository(couchdbUrl))
                .AddTransient<IJournalStageRepository>(factory => new JournalStageRepository(couchdbUrl))
                .AddTransient<IBrokerTransactionRepository>(factory => new BrokerTransactionRepository(couchdbUrl))
                .AddTransient<IImportRepository>(factory => new ImportRepository(couchdbUrl))
                .AddTransient<ISecurityRepository>(factory => new SecurityRepository(couchdbUrl))
            
                .AddTransient<IExtractor, Extractor>();
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
