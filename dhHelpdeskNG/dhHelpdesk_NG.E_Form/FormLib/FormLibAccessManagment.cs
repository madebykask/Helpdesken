using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ECT.FormLib
{
    public class FormLibAccessManagement
    {
        [AllowAnonymous]
        public async Task<List<string>> GetManagers(string country)
        {
            if(string.IsNullOrEmpty(country))
                throw new ArgumentNullException("country");

            var config = ECT.FormLib.Configurable.AccessManagment.GetConfiguration();

            var managers = new List<string>();

            var handler = new HttpClientHandler { Credentials = new NetworkCredential(config.Api.Username, config.Api.Password) };
            using(var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(config.Api.UriPath);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(String.Format("api/managers/{0}", country));
                if(response.IsSuccessStatusCode)
                {
                    managers = await response.Content.ReadAsAsync<List<string>>();
                }
            }

            return managers;
        }
    }

    public static class AsyncHelpers
    {
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch(Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            T ret = default(T);
            synch.Post(async _ =>
            {
                try
                {
                    ret = await task();
                }
                catch(Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool done;
            public Exception InnerException { get; set; }
            readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
            readonly Queue<Tuple<SendOrPostCallback, object>> items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock(items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }
                workItemsWaiting.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public void BeginMessageLoop()
            {
                while(!done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock(items)
                    {
                        if(items.Count > 0)
                        {
                            task = items.Dequeue();
                        }
                    }
                    if(task != null)
                    {
                        task.Item1(task.Item2);
                        if(InnerException != null)
                        {
                            throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                        }
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }
}