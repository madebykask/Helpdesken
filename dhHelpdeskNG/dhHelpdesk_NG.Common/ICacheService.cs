using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Common
{
    public interface ICacheService
    {
        T Get<T>(string cacheKey, Func<T> getItemCallback) where T : class;
        T Get<T>(string cacheKey, Func<T> getItemCallback, TimeSpan slidingExpiration) where T : class;
        T Get<T>(string cacheKey, Func<T> getItemCallback, DateTime absoluteExpiration) where T : class;

        Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, DateTime absoluteExpiration) where T : class;
        void Delete(string cacheKey);
    }
}
