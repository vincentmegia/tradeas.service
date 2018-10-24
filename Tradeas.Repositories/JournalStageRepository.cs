using System.Collections.Generic;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Requests;
using Tradeas.Models;

namespace Tradeas.Repositories
{
    public class JournalStageRepository : Repository, IJournalStageRepository
    {
        public JournalStageRepository(string serverAddress) : base(serverAddress, "journals-stage")
        {}
    }
}