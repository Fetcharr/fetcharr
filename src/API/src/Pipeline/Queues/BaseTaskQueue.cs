using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace Fetcharr.API.Pipeline.Queues
{
    /// <summary>
    ///   Base class for a task queue.
    /// </summary>
    /// <typeparam name="TItem">Type of item to store in the queue.</typeparam>
    public abstract class BaseTaskQueue<TItem>
        : ITaskQueue<TItem>
    {
        /// <summary>
        ///   Gets or sets the <see cref="Channel{TItem}"/>-instance to contain the queue items.
        /// </summary>
        protected abstract Channel<TItem> Queue { get; init; }

        /// <inheritdoc />
        public async ValueTask EnqueueAsync(TItem item, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            await this.Queue.Writer.WriteAsync(item, cancellationToken);
        }

        /// <inheritdoc />
        public async ValueTask<TItem> DequeueAsync(
            CancellationToken cancellationToken)
            => await this.Queue.Reader.ReadAsync(cancellationToken);

        /// <inheritdoc />
        public async IAsyncEnumerable<TItem> DequeueRangeAsync(
            int max, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            for(int i = 0; i < max; i++)
            {
                if(this.Queue.Reader.TryRead(out TItem? item))
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
        public IAsyncEnumerable<TItem> DequeueAllAsync(
            CancellationToken cancellationToken)
            => this.Queue.Reader.ReadAllAsync(cancellationToken);
    }
}