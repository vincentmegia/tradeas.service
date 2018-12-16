﻿using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider.Actors
{
    public class DirectorActor
    {
        private readonly SingleActor _singleActor;
        private readonly BatchActor _batchActor;

        public DirectorActor(SingleActor singleActor, 
                             BatchActor batchActor)
        {
            _singleActor = singleActor;
            _batchActor = batchActor;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionParameter"></param>
        /// <returns></returns>
        public TaskResult Do(TransactionParameter transactionParameter)
        {
            switch (transactionParameter.Mode)
            {
                case ImportMode.Batch:
                    _batchActor.Do(transactionParameter);
                    break;
                case ImportMode.Retry:
                    break;
                case ImportMode.Single:
                    _singleActor.Do(transactionParameter);
                    break;
            }
            return new TaskResult();
        }
    }
}