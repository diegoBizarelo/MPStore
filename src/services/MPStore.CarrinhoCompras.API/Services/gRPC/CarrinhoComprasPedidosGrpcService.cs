using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MPStore.CarrinhoCompras.API.Model;
using MPStore.ShoppingCart.API.Services.gRPC;
using MPStore.WebAPI.Core.User;

namespace MPStore.CarrinhoCompras.API.Services.gRPC
{
    [Authorize]
    public class CarrinhoComprasPedidosGrpcService : CarrinhoComprasPedidos.CarrinhoComprasPedidosBase
    {
        private readonly ILogger<CarrinhoComprasPedidosGrpcService> _logger;

        private readonly IAspNetUser _user;
        private readonly Data.ShoppingCartContext _context;

        public CarrinhoComprasPedidosGrpcService(
            ILogger<CarrinhoComprasPedidosGrpcService> logger,
            IAspNetUser user,
            Data.ShoppingCartContext context)
        {
            _logger = logger;
            _user = user;
            _context = context;
        }

        public override async Task<ClienteCarrinhoComprasClientResponse> ObterCarrinhoCompras(ObterCarrinhoComprasRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Call GetCart");

            var shoppingCart = await GetShoppingCartClient() ?? new CustomerShoppingCart();

            return MapShoppingCartClientToProtoResponse(shoppingCart);
        }

        private async Task<CustomerShoppingCart> GetShoppingCartClient()
        {
            return await _context.CustomerShoppingCart
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.ObterUsuarioId());
        }

        private static ClienteCarrinhoComprasClientResponse MapShoppingCartClientToProtoResponse(CustomerShoppingCart shoppingCart)
        {
            var shoppingCartResponse = new ClienteCarrinhoComprasClientResponse
            {
                Id = shoppingCart.Id.ToString(),
                Customerid = shoppingCart.CustomerId.ToString(),
                Total = (double)shoppingCart.Total,
            };

            foreach (var item in shoppingCart.Items)
            {
                shoppingCartResponse.Items.Add(new CarrinhoComprasItemResponse
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Image = item.Image,
                    Productid = item.ProductId.ToString(),
                    Quantity = item.Quantity,
                    Price = (double)item.Price
                });
            }

            return shoppingCartResponse;
        }
    }
}
