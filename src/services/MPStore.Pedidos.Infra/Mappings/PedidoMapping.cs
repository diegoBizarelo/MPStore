using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPStore.Pedidos.Domain.Pedidos;

namespace MPStore.Pedidos.Infra.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsOne(p => p.Endereco, e =>
            {
                e.Property(pe => pe.Logradouro)
                    .HasColumnName("Logradouro");

                e.Property(pe => pe.Numero)
                    .HasColumnName("Numero");

                e.Property(pe => pe.EnderecoSecundario)
                    .HasColumnName("EnderecoSecundario");

                e.Property(pe => pe.CEP)
                    .HasColumnName("CEP");

                e.Property(pe => pe.Cidade)
                    .HasColumnName("Cidade");

                e.Property(pe => pe.Estado)
                    .HasColumnName("Estado");
            });

            builder.Property(c => c.Codigo).UseIdentityColumn(100000, 1);

            builder.HasMany(c => c.PedidoItems)
                .WithOne(c => c.Pedido)
                .HasForeignKey(c => c.PedidoId);

            builder.ToTable("Pedidos");
        }
    }
}
