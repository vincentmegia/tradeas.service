using System.Collections.Generic;
using System.Linq;
using Tradeas.Models;

namespace Tradeas.Colfinancial.Provider
{
    /// <summary>
    /// Simple dead queue for storing failed transactions
    /// </summary>
    public static class DeadQueue
    {
        private static readonly List<Import> _queue = new List<Import>();
        private static readonly object _locker = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public static void Add(Import item)
        {
            lock (_locker)
            {
                _queue.Add(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public static void Remove(Import item)
        {
            lock (_locker)
            {
                _queue.Remove(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Clear()
        {
            lock (_locker)
            {
                _queue.Clear();
            }
        }

        /// <summary>
        /// List will only return copy
        /// </summary>
        /// <returns></returns>
        public static List<Import> Items()
        {
            lock (_queue)
            {
                //return new copy
                return _queue.ToList();
            }
        }
    }
}