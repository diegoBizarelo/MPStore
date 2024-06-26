﻿using MPStore.Core.Messages;

namespace MPStore.Cliente.API.Application.Events
{
    public class NovoClienteAdicionadoEvent : Evento
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string CPF { get; private set; }

        public NovoClienteAdicionadoEvent(Guid id, string nome, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Nome = nome;
            Email = email;
            CPF = cpf;
        }
    }
}
