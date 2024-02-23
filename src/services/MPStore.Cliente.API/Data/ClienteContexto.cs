using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MPStore.Core.Data;
using MPStore.Core.DomainObjects;
using MPStore.Core.Messages;

namespace MPStore.Cliente.API.Data
{
    public class ClienteContexto : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public ClienteContexto(DbContextOptions<ClienteContexto> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<MPStore.Cliente.API.Models.Cliente> Clientes { get; set; }
        public DbSet<MPStore.Cliente.API.Models.Endereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Evento>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClienteContexto).Assembly);
        }
        public async Task<bool> CommitAsync()
        {
            var success = await base.SaveChangesAsync() > 0;
            if (success) await _mediator.PublishEvents(this);

            return success;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublishEvents<T>(this IMediator mediator, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());


            foreach (var task in domainEvents)
            {
                await mediator.Publish(task);
            }
        }
    }
}
