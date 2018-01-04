using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.Language;

namespace Trevo.Data.Mapping.LanguageMaping
{
  public  class LanguageMapping : EntityTypeConfiguration<LanguageDetails>
    {
        public LanguageMapping()
        {
            this.HasKey(t => t.Language_Id);

            this.ToTable("Language");
            this.Property(t => t.Language_Id).HasColumnName("Language_Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ImagePath).HasColumnName("ImagePath");
            this.Property(t => t.Abbreviation).HasColumnName("Abbreviation");
        }
    }
}
