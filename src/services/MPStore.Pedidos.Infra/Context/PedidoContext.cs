using Microsoft.Extensions.Configuration;
using MPStore.Pedidos.Domain.Pedidos;
using MPStore.Core.Data;
using MPStore.Core.Mediator;
using MPStore.Core.DomainObjects;
using MPStore.Core.Messages;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace MPStore.Pedidos.Infra.Context
{
    public class PedidoContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IConfiguration _configuration;

        public PedidoContext(DbContextOptions<PedidoContext> options, IMediatorHandler mediatorHandler, IConfiguration configuration)
            : base(options)
        {
            _mediatorHandler = mediatorHandler;
            _configuration = configuration;
        }


        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<PedidoItem> PedidoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<Evento>();
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Entity<Pedido>().Property(p => p.Codigo).HasIdentityOptions(startValue: 1000);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidoContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> CommitAsync()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(entry => entry.Entity.GetType().GetProperty("DateAdded") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DateAdded").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DateAdded").IsModified = false;
                }
            }

            var sucess = await base.SaveChangesAsync() > 0;
            if (sucess) await _mediatorHandler.PublishEvents(this);

            return sucess;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublishEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var entities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = entities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            entities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
