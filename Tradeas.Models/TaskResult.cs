using System.Threading.Tasks;

namespace Tradeas.Models
{
    public class TaskResult : Result
    {
        public string Reason { get; set; }
        public string StatusCode { get; set; }

        public TaskResult()
        {}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Task<Result> Default()
        {
            return new Task<Result>(() => new TaskResult {IsSuccessful = true});
        }
    }
}
