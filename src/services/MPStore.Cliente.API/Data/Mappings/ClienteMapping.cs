using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPStore.Core.DomainObjects;

namespace MPStore.Cliente.API.Data.Mappings
{
    public class ClienteMapping : IEntityTypeConfiguration<MPStore.Cliente.API.Models.Cliente>
    {
        public void Configure(EntityTypeBuilder<MPStore.Cliente.API.Models.Cliente> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(c => c.CPF).IsRequired()
                    .HasColumnType($"varchar(50)");

            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.Endereco)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType($"varchar({Email.EnderecoMaxLength})");
            });

            builder.HasOne(c => c.Endereco)
                .WithOne(c => c.Cliente);

            builder.ToTable("Cliente");
        }

        
    }
}
