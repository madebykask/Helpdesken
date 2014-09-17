namespace DH.Helpdesk.Dal.Infrastructure
{
    using System;

    public abstract class Disposable : IDisposable
    {
        private bool isDisposed;

        ~Disposable()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void DisposeCore();

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                this.DisposeCore();
            }

            this.isDisposed = true;
        }
    }
}
