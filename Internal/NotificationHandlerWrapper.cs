using Microsoft.Extensions.DependencyInjection;

namespace MediatorClone.Internal;

internal interface INotificationHandlerWrapper
{
    Task Handle(INotification notification, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}

internal sealed class NotificationHandlerWrapper<TNotification> : INotificationHandlerWrapper
    where TNotification : INotification
{
    public async Task Handle(INotification notification, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var handlers = serviceProvider.GetServices<INotificationHandler<TNotification>>();
        foreach (var handler in handlers)
            await handler.Handle((TNotification)notification, cancellationToken);
    }
}
