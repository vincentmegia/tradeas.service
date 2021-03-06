﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using log4net;
using Tradeas.Models;
using Tradeas.Repositories;

namespace Tradeas.Colfinancial.Provider.Processors
{
    public class ImportProcessor
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ImportProcessor));
        private readonly IImportRepository _importRepository;
        private readonly IImportTrackerRepository _importTrackerRepository;
        private readonly IImportHistoryRepository _importHistoryRepository;

        public ImportProcessor(IImportRepository importRepository, 
                               IImportTrackerRepository importTrackerRepository, 
                               IImportHistoryRepository importHistoryRepository)
        {
            _importRepository = importRepository;
            _importTrackerRepository = importTrackerRepository;
            _importHistoryRepository = importHistoryRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult Process(ImportMode importMode)
        {
            _importHistoryRepository.Add(new ImportHistory("broker-transactions", "broker.service"));

            var importTrackers = _importTrackerRepository
                .GetAll()
                .GetData<List<ImportTracker>>()
                .ToList();

            if (importMode == ImportMode.Retry)
                importTrackers = importTrackers.FindAll(importTracker =>
                    importTracker.Status.Equals("Retry", StringComparison.CurrentCultureIgnoreCase));

            var imports = _importRepository
                .GetAll()
                .Result
                .GetData<List<Import>>()
                .ToList();
            
            imports = imports.FindAll(import => !importTrackers.Contains(new ImportTracker(import.Symbol)));
            
           
            var taskResult = new TaskResult {IsSuccessful = true};
            taskResult.SetData(imports);
            return taskResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public void PurgeTrackers(TransactionParameter transactionParameter)
        {
            if (transactionParameter.NoPurge != null && transactionParameter.NoPurge.Value) return;
            
            var importsHistory = _importHistoryRepository
                .GetByDate(DateTime.Now)
                .GetData<ImportHistory>();
            if (importsHistory == null || (transactionParameter.FromDate != null && transactionParameter.ToDate != null))
            {
                var importTrackersResponse = _importTrackerRepository.GetAll();
                var importTrackers = importTrackersResponse.GetData<List<ImportTracker>>();
                foreach (var importTracker in importTrackers)
                {
                    _importTrackerRepository.DeleteAsync(importTracker);
                }
            }
            Logger.Info($"purge completed.");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>c 
        public bool IsCompleted()
        {
            var importTrackers = _importTrackerRepository
                .GetAll()
                .GetData<List<ImportTracker>>();

            var imports = _importRepository
                .GetAll()
                .Result
                .GetData<List<Import>>()
                .OrderBy(i => i.Symbol);

            if (importTrackers.Count == imports.Count())
                return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TaskResult AddTracker(ImportTracker importTracker)
        {
            _importTrackerRepository.PostAsync(importTracker);
            return new TaskResult {IsSuccessful = true};
        }
    }
}