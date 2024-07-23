using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100);
            builder.Property(p => p.Description)
             .IsRequired();
            builder.Property(p => p.PictureUrl)
          .IsRequired();

          
            builder.HasOne(p => p.ProductType)
              .WithMany()
              .HasForeignKey(p => p.ProductTypeId);

        }
    }
}
