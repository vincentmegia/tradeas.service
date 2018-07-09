using System;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public interface IImportHistoryRepository
    {
        TaskResult GetByDate(DateTime date);
        TaskResult Add(ImportHistory importHistory);
    }
}