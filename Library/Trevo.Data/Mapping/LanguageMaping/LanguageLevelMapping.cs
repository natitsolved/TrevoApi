using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trevo.Core.Model.Language;

namespace Trevo.Data.Mapping.LanguageMaping
{
  public  class LanguageLevelMapping : EntityTypeConfiguration<LanguageLevel>
    {
        public LanguageLevelMapping()
        {
            this.HasKey(t => t.LagLevel_Id);

            this.ToTable("LanguageLevel");
            this.Property(t => t.LagLevel_Id).HasColumnName("LagLevel_Id");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
