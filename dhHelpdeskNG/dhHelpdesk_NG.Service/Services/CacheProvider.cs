namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Runtime.Caching;

    public interface ICacheProvider
    {
        object Get(string key);
        void Set(string key, object data, int cacheTime);
        bool IsSet(string key);
        void Invalidate(string key);
    }

    public class CacheProvider : ICacheProvider
    {
        private ObjectCache Cache { get { return MemoryCache.Default; } }

        public object Get(string key)
        {
            return this.Cache[key];
        }

        public void Set(string key, object data, int cacheTime)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);

            this.Cache.Add(new CacheItem(key, data), policy);
        }

        public bool IsSet(string key)
        {
            return (this.Cache[key] != null);
        }

        public void Invalidate(string key)
        {
            this.Cache.Remove(key);
        }
    }
}
