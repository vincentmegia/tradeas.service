using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Service.Api.Processors
{
    public interface IJournalStageProcessor
    {
        Task<Result> Process();
    }
}
