using System;
using System.Threading.Tasks;
using System.Web;
using DH.Helpdesk.Common;

namespace DH.Helpdesk.WebApi.Infrastructure.Cache
{
	public class WebCacheService : ICacheService
	{

		public T Get<T>(string cacheKey, Func<T> getItemCallback, TimeSpan slidingExpiration) where T : class
		{
			if (!IsCacheEnabled)
				return getItemCallback();
			var item = HttpRuntime.Cache.Get(cacheKey) as T;
			if (item == null)
			{
				item = getItemCallback();
				HttpRuntime.Cache.Insert(cacheKey, item, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration);
			}
			return item;
		}

		public T Get<T>(string cacheKey, Func<T> getItemCallback, DateTime absoluteExpiration) where T : class
		{
			if (!IsCacheEnabled)
				return getItemCallback();
			var item = HttpRuntime.Cache.Get(cacheKey) as T;
			if (item == null)
			{
				item = getItemCallback();

				HttpRuntime.Cache.Insert(cacheKey, item, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
			}
			return item;
		}

		public async Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> getItemCallback, DateTime absoluteExpiration) where T : class
		{
			if (!IsCacheEnabled)
				return await getItemCallback();
			var item = await Task.FromResult(HttpRuntime.Cache.Get(cacheKey) as T);
			if (item == null)
			{
				item = await getItemCallback();

				HttpRuntime.Cache.Insert(cacheKey, item, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
			}
			return item;
		}

		public T Get<T>(string cacheKey, Func<T> getItemCallback) where T : class
		{
			return Get(cacheKey, getItemCallback, TimeSpan.FromMinutes(30)); //TODO: Move default time to config
		}

		public void Delete(string cacheKey)
		{
			HttpRuntime.Cache.Remove(cacheKey);
		}


		private bool IsCacheEnabled => true;
	}

}
