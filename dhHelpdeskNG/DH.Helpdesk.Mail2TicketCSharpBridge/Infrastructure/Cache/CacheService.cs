using DH.Helpdesk.Services.Services.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Mail2TicketCSharpBridge.Infrastructure.Cache
{
    public class CacheService : ICacheService
    {
        private bool IsCacheEnabled => true;

        public T Get<T>(string cacheKey, Func<T> getItemCallback, TimeSpan slidingExpiration) where T : class
        {
            return getItemCallback();
        }

        public T Get<T>(string cacheKey, Func<T> getItemCallback, DateTime absoluteExpiration) where T : class
        {
            return getItemCallback();
        }

        public async Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, DateTime absoluteExpiration)
            where T : class
        {
            return await getItemCallback();
        }

        public T Get<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            return Get(cacheKey, getItemCallback, TimeSpan.FromMinutes(30));
        }

        public void Delete(string cacheKey)
        {
            //HttpRuntime.Cache.Remove(cacheKey);
        }
    }
}

