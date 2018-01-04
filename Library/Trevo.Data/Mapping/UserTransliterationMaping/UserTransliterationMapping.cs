using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.UserTransliteration;

namespace Trevo.Data.Mapping.UserTransliterationMaping
{
    public class UserTransliterationMapping : EntityTypeConfiguration<UserTransliterationDetails>
    {

        public UserTransliterationMapping()
        {
            this.HasKey(a => a.TransliterationId);

            this.ToTable("UserTransliteration");
            this.Property(t => t.SpellCheckCount).HasColumnName("SpellCheckCount");
            this.Property(t => t.TranslateCount).HasColumnName("TranslateCount");
            this.Property(t => t.TransliterationId).HasColumnName("TransliterationId");
            this.Property(t => t.TTSCount).HasColumnName("TTSCount");
            this.Property(t => t.User_Id).HasColumnName("User_Id");
            this.Property(t => t.FavouritesCount).HasColumnName("FavouritesCount");
        }
    }
}
