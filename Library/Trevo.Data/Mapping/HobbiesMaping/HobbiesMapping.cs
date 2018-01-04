using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.Hobbies;

namespace Trevo.Data.Mapping.HobbiesMaping
{
  public  class HobbiesMapping : EntityTypeConfiguration<HobbiesDetails>
    {
        public HobbiesMapping()
        {
            this.HasKey(t => t.HobbiesId);

            this.ToTable("Hobbies");
            this.Property(t => t.HobbiesId).HasColumnName("HobbiesId");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
