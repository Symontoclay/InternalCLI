using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{
    public abstract class Disposable : IDisposable
    {
        /// <inheritdoc/>
        public void Dispose()
        {
            lock (_lockObj)
            {
                if (_isDisposed)
                {
                    return;
                }

                _isDisposed = true;
            }

            OnDisposing();
        }

        public bool IsDisposed
        {
            get
            {
                lock (_lockObj)
                {
                    return _isDisposed;
                }
            }
        }

        private bool _isDisposed;
        private readonly object _lockObj = new object();

        protected virtual void OnDisposing()
        {
        }
    }
}
