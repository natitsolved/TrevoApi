using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.UserTransliteration;

namespace Trevo.Data.Mapping.UserTransliterationMaping
{
    public class TransliterationMapping : EntityTypeConfiguration<TransliterationDetails>
    {
        public TransliterationMapping()
        {
            this.HasKey(a => a.Id);

            this.ToTable("TransliterationDetails");
            this.Property(t => t.Details).HasColumnName("Details");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IsFavourite).HasColumnName("IsFavourite");
            this.Property(t => t.IsMoment).HasColumnName("IsMoment");
            this.Property(t => t.IsSpellCheck).HasColumnName("IsSpellCheck");
            this.Property(t => t.IsTranslate).HasColumnName("IsTranslate");
            this.Property(t => t.IsTTS).HasColumnName("IsTTS");
            this.Property(t => t.User_Id).HasColumnName("User_Id");
        }
    }
}
