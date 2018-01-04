using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.Favourites;

namespace Trevo.Data.Mapping.FavouritesMaping
{
    public  class FavouritesMapping : EntityTypeConfiguration<FavouritesDetails>
    {

        public FavouritesMapping()
        {
            this.HasKey(t => t.FavouritesId);

            this.ToTable("Favourites");
            this.Property(t => t.FavouritesId).HasColumnName("FavouritesId");
            this.Property(t => t.FavouriteUserId).HasColumnName("FavouriteUserId");
            this.Property(t => t.IsSender).HasColumnName("IsSender");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.MomentId).HasColumnName("MomentId");
            this.Property(t => t.SenderRecieverId).HasColumnName("SenderRecieverId");
            this.Property(t => t.AddedDate).HasColumnName("AddedDate");
            this.Property(t => t.LocalMessageId).HasColumnName("LocalMessageId");
        }
    }
}
