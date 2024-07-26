using System.Threading.Channels;

namespace Fetcharr.API.Pipeline.Queues
{
    /// <summary>
    ///   Base class for a bounded task queue, i.e. a task queue with a pre-defined capacity of <paramref name="capacity"/>.
    /// </summary>
    /// <typeparam name="TItem">Type of item to store in the queue.</typeparam>
    /// <param name="capacity">Maximum amount of items to keep in the queue.</param>
    /// <param name="fullMode">Behaviour for when the queue is full. See <see cref="BoundedChannelFullMode"/>.</param>
    public abstract class BaseBoundedTaskQueue<TItem>(
        int capacity,
        BoundedChannelFullMode fullMode = BoundedChannelFullMode.Wait)
        : BaseTaskQueue<TItem>
    {
        protected override Channel<TItem> Queue { get; init; } =
            Channel.CreateBounded<TItem>(new BoundedChannelOptions(capacity)
            {
                FullMode = fullMode
            });
    }
}