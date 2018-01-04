using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trevo.Core.Model.Country;

namespace Trevo.Data.Mapping.CountryMaping
{
   public class CountryMapping : EntityTypeConfiguration<Country>
    {
        public CountryMapping()
        {
            this.HasKey(t => t.Country_Id);

            this.ToTable("Country");
            this.Property(t => t.Country_Id).HasColumnName("Country_Id");
            this.Property(t => t.Flag_Icon).HasColumnName("Flag_Icon");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ImagePath).HasColumnName("ImagePath");
        }
    }
}
