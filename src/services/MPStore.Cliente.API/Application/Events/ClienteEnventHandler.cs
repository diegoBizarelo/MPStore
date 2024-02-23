using MediatR;

namespace MPStore.Cliente.API.Application.Events
{
    public class ClienteEnventHandler : INotificationHandler<NovoClienteAdicionadoEvent>
    {
        public Task Handle(NovoClienteAdicionadoEvent notification, CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*****************************************************************");
            Console.WriteLine($"O evento agregado {notification.AggregateId} foi manipulado!");
            Console.WriteLine("*****************************************************************");
            Console.ForegroundColor = ConsoleColor.White;

            return Task.CompletedTask;
        }
    }
}
