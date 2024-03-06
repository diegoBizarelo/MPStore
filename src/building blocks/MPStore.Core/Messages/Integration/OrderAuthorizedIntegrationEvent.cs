﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPStore.Core.Messages.Integration
{
    public class OrderAuthorizedIntegrationEvent : IntegrationEvent
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public IDictionary<Guid, int> Items { get; private set; }

        public OrderAuthorizedIntegrationEvent(Guid customerId, Guid orderId, IDictionary<Guid, int> items)
        {
            CustomerId = customerId;
            OrderId = orderId;
            Items = items;
        }
    }
}
