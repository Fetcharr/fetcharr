using System.Threading.Channels;

namespace Fetcharr.API.Pipeline.Queues
{
    /// <summary>
    ///   Base class for a unbounded task queue, i.e. a task queue with any capacity limit.
    /// </summary>
    /// <typeparam name="TItem">Type of item to store in the queue.</typeparam>
    public abstract class BaseUnboundedTaskQueue<TItem>
        : BaseTaskQueue<TItem>
    {
        protected override Channel<TItem> Queue { get; init; } =
            Channel.CreateUnbounded<TItem>(new UnboundedChannelOptions());
    }
}