using System;
using System.Collections.Generic;

namespace Tradeas.Models
{
    public class Result<T>
    {
        public List<String> Messages { get; set; }
        public Boolean IsSuccessful { get; set; }
        public Exception Exception { get; set; }
        public T Instance { get; set; }

        public Result()
        {
            Messages = new List<string>();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Result`1"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Result`1"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Result: Messages={0}, IsSuccessful={1}, Exception={2}, Instance={3}]", Messages, IsSuccessful, Exception, Instance);
        }
    }
}
