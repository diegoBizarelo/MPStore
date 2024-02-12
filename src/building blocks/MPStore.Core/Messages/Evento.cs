using System;
using MediatR;
using MPStore.Core.Messagem;

namespace MPStore.Core.Messages
{
    public class Evento : Mensagem, INotification
    {
        public DateTime TimeStamp { get; private set; }

        protected Evento ()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
