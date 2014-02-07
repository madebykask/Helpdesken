namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;

    public class Disposable : IDisposable
    {
        private bool _isDisposed;

        ~Disposable()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(!this._isDisposed && disposing)
            {
                this.DisposeCore();
            }

            this._isDisposed = true;
        }

        protected virtual void DisposeCore()
        {

        }
    }
}
