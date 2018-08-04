using System.Threading.Tasks;
using Tradeas.Models;

namespace Tradeas.Web.Api.Processors
{
    public interface IJournalStageProcessor
    {
        Task<Result> Process();
    }
}
