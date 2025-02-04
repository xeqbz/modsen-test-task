using LibraryApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Infrastructure.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
            builder.Property(b => b.Title).IsRequired().HasMaxLength(255);
            builder.Property(b => b.Genre).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Description).IsRequired().HasMaxLength(1000);
            builder.HasOne(b => b.Author).WithMany(a => a.Books).HasForeignKey(b => b.AuthorId);
        }
    }
}
