using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPStore.Core.Messagem
{
    public abstract class Mensagem
    {
        public string TipoMensagem { get; protected set; }
        public Guid AggregateId { get; protected set; }

        public Mensagem() 
        {
            TipoMensagem = GetType().Name;
        }
    }
}
