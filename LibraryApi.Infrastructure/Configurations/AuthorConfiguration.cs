using LibraryApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Infrastructure.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(a => a.LastName).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Country).IsRequired().HasMaxLength(100);
            builder.Property(a => a.DateOfBirth).IsRequired();
        }
    }
}
