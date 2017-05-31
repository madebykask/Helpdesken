namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Runtime.Caching;
    using System.Collections.Generic;

    public interface ICacheProvider
    {
        object Get(string key);
        void Set(string key, object data, int cacheTime);
        bool IsSet(string key);
        void Invalidate(string key);
        Dictionary<string, string> GetByStartsWith(string startsWith);
        void InvalidateStartsWith(string startsWith);
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

        public void InvalidateStartsWith(string startsWith)
        {
            foreach (var item in this.Cache)
            {
                if (item.Key.ToLower().StartsWith(startsWith.ToLower()))
                    this.Cache.Remove(item.Key);
            }
        }

        public Dictionary<string, string> GetByStartsWith(string startsWith)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var item in MemoryCache.Default)
            {
                if (item.Key.ToLower().StartsWith(startsWith.ToLower()))
                    dictionary.Add(item.Key, item.Value.ToString());
            }

            return dictionary;
        }
    }
}
