using System.ComponentModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCouch;
using Tradeas.Colfinancial.Provider;
using Tradeas.Colfinancial.Provider.Actors;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Navigators;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Colfinancial.Provider.Scrapers;
using Tradeas.Colfinancial.Provider.Simulators;
using Tradeas.Repositories;
using Tradeas.Web.Api.Builders;
using Tradeas.Web.Api.Processors;

namespace Tradeas.Web.Api
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddControllersAsServices()
                    .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ");

            //var couchdbUrl = "http://127.0.0.1:5984";
            //var couchdbUrl = "http://104.215.158.74:5984/";
            var couchdbUrl = Configuration["CouchDb:Url"];
            services
                .AddTransient<IJournalBuilder, JournalBuilder>()
                .AddTransient<ITransactionScraper, TradeTransactionScraper>()
                .AddTransient<IPortfolioScraper, PortfolioScraper>()
                .AddTransient<IJournalProcessor, JournalProcessor>()
                .AddTransient<IJournalStageProcessor, JournalStageProcessor>()
                .AddTransient<ITransactionBuilder, TransactionBuilder>()
                .AddTransient<ITransactionProcessor, TransactionProcessor>()
            
                .AddTransient(typeof(LoginNavigator))
                .AddTransient(typeof(BrokerTabNavigator))
                
                .AddTransient(typeof(LoginSimulator))
                .AddTransient(typeof(BrokerTransactionSimulator))
            
                .AddTransient(typeof(BrokerTransactionScraper))
                .AddTransient(typeof(BrokerTransactionBuilder))
                .AddTransient(typeof(BrokerTransactionProcessor))
                .AddTransient(typeof(ImportProcessor))
            
                .AddTransient<IJournalRepository>(factory => new JournalRepository(couchdbUrl))
                .AddTransient<ITransactionRepository>(factory => new TransactionRepository(couchdbUrl))
                .AddTransient<IJournalStageRepository>(factory => new JournalStageRepository(couchdbUrl))
                .AddTransient<IBrokerTransactionRepository>(factory => new BrokerTransactionRepository(couchdbUrl))
                .AddTransient<IImportRepository>(factory => new ImportRepository(couchdbUrl))
                .AddTransient<ISecurityRepository>(factory => new SecurityRepository(couchdbUrl))
                .AddTransient<IImportTrackerRepository>(factory => new ImportTrackerRepository(couchdbUrl))
                .AddTransient<IImportHistoryRepository>(factory => new ImportHistoryRepository(couchdbUrl))
            
                .AddTransient(typeof(BrokerActor))
                .AddTransient(typeof(BatchProcessor))
                .AddTransient(typeof(TaskProcessor))
            
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
