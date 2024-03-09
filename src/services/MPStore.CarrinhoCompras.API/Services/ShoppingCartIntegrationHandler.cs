using Microsoft.EntityFrameworkCore;
using MPStore.CarrinhoCompras.API.Data;
using MPStore.Core.Messages.Integration;
using MPStore.MessageBus;

namespace MPStore.CarrinhoCompras.API.Services
{
    public class ShoppingCartIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public ShoppingCartIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.SubscribeAsync<PedidoFeitoIntegrationEvent>("PedidoFeito", RemoveShoppingCart);
        }

        private async Task RemoveShoppingCart(PedidoFeitoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();

            var shoppingCart = await context.CustomerShoppingCart
                .FirstOrDefaultAsync(c => c.CustomerId == message.ClienteId);

            if (shoppingCart != null)
            {
                context.CustomerShoppingCart.Remove(shoppingCart);
                await context.SaveChangesAsync();
            }
        }
    }
}
