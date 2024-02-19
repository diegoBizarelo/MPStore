using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using MPStore.Catalogo.API.Models;
using MPStore.Core.Data;
using NetDevPack.Messaging;

namespace MPStore.Catalogo.API.Data
{
    public class CatalogoContexto : DbContext, IUnitOfWork
    {
        public DbSet<Produto> Produtos { get; set; }

        public CatalogoContexto(DbContextOptions<CatalogoContexto> options)
         : base(options) 
        { }

        

        public async Task<bool> CommitAsync()
        {
            return await base.SaveChangesAsync() > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContexto).Assembly);
        }
    }
}
