using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Fetcharr.API.Pipeline
{
    /// <summary>
    ///   Represents a thread-safe FIFO (first-in, first-out) set of objects.
    /// </summary>
    /// <typeparam name="T">Type of item to store in the queue.</typeparam>
    public class ConcurrentSetQueue<T>(
        IEqualityComparer<T>? comparer = null)
        : IProducerConsumerCollection<T>
        , IReadOnlyCollection<T>
    {
        private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.SupportsRecursion);

        private readonly Queue<T> _queue = [];

        private readonly IEqualityComparer<T> _comparer = comparer ?? EqualityComparer<T>.Default;

        /// <summary>
        ///   Enqueue an item into the <see cref="ConcurrentSetQueue{T}" />.
        ///   If an item equal to <paramref name="item"/> already exists within the <see cref="ConcurrentSetQueue{T}" />, nothing is done.
        /// </summary>
        /// <param name="item">The item to add to the <see cref="ConcurrentSetQueue{T}"/></param>
        public void Enqueue(T item)
            => this.TryAdd(item);

        public T Dequeue()
        {
            this._lock.EnterUpgradeableReadLock();

            try
            {
                SpinWait spinner = default;

                while(!this._queue.TryPeek(out _))
                {
                    spinner.SpinOnce();
                }

                try
                {
                    this._lock.EnterWriteLock();
                    return this._queue.Dequeue();
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
            finally
            {
                this._lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        ///   Dequeue all the items from the <see cref="ConcurrentSetQueue{T}" /> and return them.
        /// </summary>
        public IEnumerable<T> DequeueAll()
        {
            this._lock.EnterWriteLock();

            try
            {
                while(this._queue.TryDequeue(out T? item))
                {
                    yield return item;
                }
            }
            finally
            {
                if(this._lock.IsWriteLockHeld)
                {
                    this._lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        ///   Attemps to add an item to the <see cref="ConcurrentSetQueue{T}"/>.
        /// </summary>
        /// <param name="item">The item to add to the <see cref="ConcurrentSetQueue{T}"/></param>
        /// <returns><see langword="true" /> if the object was added successfully; otherwise, <see langword="false" />.</returns>
        public bool TryEnqueue(T item)
            => this.TryAdd(item);

        /// <summary>
        ///   Attemps to remove and return an item to the <see cref="ConcurrentSetQueue{T}"/>.
        /// </summary>
        /// <param name="result">
        ///   If the method returns <see langword="true" />, contains the item returned;
        ///   otherwise, <see langword="null" />.
        /// </param>
        /// <returns>
        ///   <see langword="true" /> if the object was removed successfully; otherwise, <see langword="false" />.
        /// </returns>
        public bool TryDequeue([MaybeNullWhen(false)] out T result)
            => this.TryTake(out result);

        /// <inheritdoc cref="IProducerConsumerCollection{T}.TryAdd" />
        public bool TryAdd(T item)
        {
            this._lock.EnterWriteLock();

            try
            {
                if(this.ContainsItem(item))
                {
                    return false;
                }

                this._queue.Enqueue(item);
                return true;
            }
            finally
            {
                if(this._lock.IsWriteLockHeld)
                {
                    this._lock.ExitWriteLock();
                }
            }
        }

        /// <inheritdoc cref="IProducerConsumerCollection{T}.TryTake" />
        public bool TryTake([MaybeNullWhen(false)] out T item)
        {
            this._lock.EnterWriteLock();

            try
            {
                return this._queue.TryDequeue(out item);
            }
            finally
            {
                if(this._lock.IsWriteLockHeld)
                {
                    this._lock.ExitWriteLock();
                }
            }
        }

        /// <inheritdoc cref="IProducerConsumerCollection{T}.ToArray" />
        public T[] ToArray()
        {
            this._lock.EnterReadLock();

            try
            {
                return [.. this._queue];
            }
            finally
            {
                if(this._lock.IsReadLockHeld)
                {
                    this._lock.ExitReadLock();
                }
            }
        }

        private bool ContainsItem(T item)
        {
            this._lock.EnterReadLock();

            try
            {
                return this._queue.Contains(item, this._comparer);
            }
            finally
            {
                if(this._lock.IsReadLockHeld)
                {
                    this._lock.ExitReadLock();
                }
            }
        }

        /// <inheritdoc cref=" Queue{T}.Count" />
        public int Count
        {
            get
            {
                this._lock.EnterReadLock();

                try
                {
                    return this._queue.Count;
                }
                finally
                {
                    if(this._lock.IsReadLockHeld)
                    {
                        this._lock.ExitReadLock();
                    }
                }
            }
        }

        /// <inheritdoc cref="ICollection.IsSynchronized" />
        public bool IsSynchronized => false;

        /// <inheritdoc cref="ICollection.SyncRoot" />
        public object SyncRoot => throw new NotSupportedException();

        /// <inheritdoc cref="IProducerConsumerCollection{T}.CopyTo" />
        void IProducerConsumerCollection<T>.CopyTo(T[] array, int index)
            => this._queue.CopyTo(array, index);

        /// <inheritdoc cref="ICollection.CopyTo" />
        void ICollection.CopyTo(Array array, int index)
            => this._queue.ToArray().CopyTo(array, index);

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
            => this._queue.GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
            => this._queue.GetEnumerator();
    }
}