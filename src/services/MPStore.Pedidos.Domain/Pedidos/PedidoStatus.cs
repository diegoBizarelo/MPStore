using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPStore.Pedidos.Domain.Pedidos
{
    public enum PedidoStatus
    {
        Autorizado = 1,
        Pago = 2,
        Recusado = 3,
        Enderegue = 4,
        Cancelado = 5
    }
}
