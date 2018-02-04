using System;
using System.Collections.Generic;

namespace Tradeas.Models
{
    public abstract class Result
    {
        public List<String> Messages { get; set; }
        public bool? IsSuccessful { get; set; }
        public Exception Exception { get; set; }
        private object _data;

        protected Result()
        {
            Messages = new List<string>();
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public Result SetData<T>(T data)
        {
            _data = data;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The data.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T GetData<T>()
        {
            return (T)_data;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Result`1"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Tradeas.Models.Result`1"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Result: Messages={0}, IsSuccessful={1}, Exception={2}, Data={3}]", Messages, IsSuccessful, Exception, _data);
        }
    }
}
