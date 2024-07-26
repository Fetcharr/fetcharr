using System.Runtime.CompilerServices;

namespace Fetcharr.API.Pipeline.Queues
{
    /// <summary>
    ///   Base class for a set queue of tasks, i.e. queue with only unique elements.
    /// </summary>
    /// <remarks>
    ///   Since the queue uses <see cref="ConcurrentSetQueue{TItem}"/> under the hood, order of the queue cannot be guranteed.
    /// </remarks>
    /// <typeparam name="TItem">Type of item to store in the queue.</typeparam>
    public abstract class BaseUniqueTaskQueue<TItem>
        : ITaskQueue<TItem>
    {
        private readonly ConcurrentSetQueue<TItem> _queue = [];

        public async ValueTask EnqueueAsync(TItem item, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            this._queue.Enqueue(item);

            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public async ValueTask<TItem> DequeueAsync(
            CancellationToken cancellationToken)
            => await ValueTask.FromResult(this._queue.Dequeue());

        /// <inheritdoc />
        public async IAsyncEnumerable<TItem> DequeueRangeAsync(
            int max, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            for(int i = 0; i < max; i++)
            {
                if(this._queue.TryDequeue(out TItem? item))
                {
                    yield return item;
                }
                else
                {
                    break;
                }
            }

            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<TItem> DequeueAllAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach(TItem item in this._queue.DequeueAll())
            {
                yield return item;
            }

            await Task.CompletedTask;
        }
    }
}