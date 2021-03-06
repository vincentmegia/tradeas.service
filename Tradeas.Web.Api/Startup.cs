﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenQA.Selenium;
using Tradeas.Colfinancial.Provider;
using Tradeas.Colfinancial.Provider.Actors;
using Tradeas.Colfinancial.Provider.Builders;
using Tradeas.Colfinancial.Provider.Navigators;
using Tradeas.Colfinancial.Provider.Processors;
using Tradeas.Colfinancial.Provider.Scrapers;
using Tradeas.Colfinancial.Provider.Simulators;
using Tradeas.Repositories;
using Tradeas.Security;
using Tradeas.Web.Api.Builders;
using Tradeas.Web.Api.Processors;
using Tradeas.Web.Api.Services;

namespace Tradeas.Web.Api
{
    public class Startup
    {
        public IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }
        public IServiceProvider ServiceProvider { get; }
        
        public Startup(IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            Configuration = configuration;
            var log4NetFile = configuration["Log4Net:Path"];
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(File.OpenRead(log4NetFile));
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(Hierarchy));
            XmlConfigurator.Configure(repo, xmlDocument["log4net"]);
        }

        
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors()
                .AddDistributedMemoryCache()
                .AddSession()
                .AddMvc()
                .AddControllersAsServices()
                .AddJsonOptions(options => {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
            });
            
            var couchdbUrl = Configuration["CouchDb:Url"];
            var username = Configuration["CouchDb:LoginCredential:Username"];
            var password = Configuration["CouchDb:LoginCredential:Password"];
            if (username != null && password != null)
            {
                username = Crypter.DecryptString(Configuration["CouchDb:LoginCredential:Username"], "tr@d3@s.as1n");
                password = Crypter.DecryptString(Configuration["CouchDb:LoginCredential:Password"], "tr@d3@s.as1n");
                couchdbUrl = string.Format(couchdbUrl, username, password);
            }

            var key = Encoding.ASCII.GetBytes(Configuration["JwtPassPhrase"]);

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
                .AddTransient<ITradeasRepository>(factory => new TradeasRepository(couchdbUrl))
                .AddTransient<IImportRepository>(factory => new ImportRepository(couchdbUrl))
                .AddTransient<ISecurityRepository>(factory => new SecurityRepository(couchdbUrl))
                .AddTransient<IImportTrackerRepository>(factory => new ImportTrackerRepository(couchdbUrl))
                .AddTransient<IImportHistoryRepository>(factory => new ImportHistoryRepository(couchdbUrl))

                .AddSingleton<IAuthenticationService, AuthenticationService>()
                .AddSingleton<IJwtService, JwtService>()
                .AddSingleton<IHostedService, BrokerExtractService>()
                
                .AddTransient(typeof(CancellationTokenSource))
                .AddTransient(typeof(WebDriverFactory))
                .AddTransient<List<TaskActor>>(factory =>
                {
                    var webDriverFactory = (WebDriverFactory)services.BuildServiceProvider().GetService(typeof(WebDriverFactory));
                    var brokerTransactionScraper =
                        (BrokerTransactionScraper) services.BuildServiceProvider().GetService(typeof(BrokerTransactionScraper));
                    var cancellationTokenSource =
                        (CancellationTokenSource) services.BuildServiceProvider().GetService(typeof(CancellationTokenSource));
                    var workers = Convert.ToInt32(Configuration["WorkerCount"]);
                    var taskActors = new List<TaskActor>();
                    for (var index = 0; index <= workers - 1; index++)
                        taskActors.Add(new TaskActor(webDriverFactory, brokerTransactionScraper, cancellationTokenSource));

                    return taskActors;
                })
                
            
                .AddTransient(typeof(DirectorActor))
                .AddTransient(typeof(RecoveryActor))
                .AddTransient(typeof(BatchActor))
                .AddTransient(typeof(SingleActor))
                .AddTransient(typeof(TaskActor))
                .AddTransient(typeof(BatchProcessor))
                .AddTransient(typeof(TaskProcessor))
                
                .AddTransient<IExtractor, Extractor>()

                .AddAuthentication()
                .AddJwtBearer(option =>
                {
                    option.RequireHttpsMetadata = false;
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        
        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                    .UsePathBase("/api");
            }

            app.UseCors(builder =>
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials())
                .UseAuthentication()
                .UseMvc();
        }
    }
}
