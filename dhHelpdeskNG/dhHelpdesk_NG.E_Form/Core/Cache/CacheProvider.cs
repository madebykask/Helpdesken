using System;
using System.Linq;
using System.Runtime.Caching;

namespace DH.Helpdesk.EForm.Core.Cache
{
    public class CacheProvider
    {
        private ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }

        public object Get(string key)
        {
            return Cache[key];
        }

        public void Set(string key, object data, int cacheTime)
        {
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime) };

            Cache.Add(new CacheItem(key, data), policy);
        }

        public bool IsSet(string key)
        {
            return (Cache[key] != null);
        }

        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }

        public void InvalidateAll()
        {
            var cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach(string cacheKey in cacheKeys)
                MemoryCache.Default.Remove(cacheKey);
        }
    }
}