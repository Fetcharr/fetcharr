namespace Fetcharr.API.Pipeline
{
    /// <summary>
    ///   Contract for a task queue; consumed by background services and other async operations.
    /// </summary>
    /// <typeparam name="TItem">Type of item to store in the queue.</typeparam>
    public interface ITaskQueue<TItem>
    {
        /// <summary>
        ///   Enqueue a new item onto the queue.
        /// </summary>
        /// <param name="item">Item to append onto the queue.</param>
        ValueTask EnqueueAsync(TItem item, CancellationToken cancellationToken);

        /// <summary>
        ///   Dequeue the oldest item off the queue and return it. If the queue is empty, this method is blocking.
        /// </summary>
        ValueTask<TItem> DequeueAsync(CancellationToken cancellationToken);

        /// <summary>
        ///   Dequeue items off the queue until it is empty or until <paramref name="max"/> items have been dequeued.
        /// </summary>
        /// <param name="max">Maximum amount of items to dequeue from the queue.</param>
        /// <returns><see cref="IAsyncEnumerable{TItem}"/>-instance of, at most, <paramref name="max"/> items.</returns>
        IAsyncEnumerable<TItem> DequeueRangeAsync(int max, CancellationToken cancellationToken);

        /// <summary>
        ///   Dequeue all items off the queue and return them.
        /// </summary>
        /// <returns><see cref="IAsyncEnumerable{TItem}"/>-instance of all items within the queue.</returns>
        IAsyncEnumerable<TItem> DequeueAllAsync(CancellationToken cancellationToken);
    }
}