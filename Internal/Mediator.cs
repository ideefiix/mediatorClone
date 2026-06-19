using System.Collections.Concurrent;

namespace MediatorClone.Internal;

internal sealed class Mediator : IPublisher
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly ConcurrentDictionary<Type, INotificationHandlerWrapper> NotificationWrappers = new();

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Publish(INotification notification, CancellationToken cancellationToken = default)
    {
        var wrapper = NotificationWrappers.GetOrAdd(notification.GetType(), t =>
        {
            var wrapperType = typeof(NotificationHandlerWrapper<>).MakeGenericType(t);
            return (INotificationHandlerWrapper)Activator.CreateInstance(wrapperType)!;
        });

        return wrapper.Handle(notification, _serviceProvider, cancellationToken);
    }
}
