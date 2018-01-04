using System.Data.Entity.ModelConfiguration;
using Trevo.Core.Model.Chat;

namespace Trevo.Data.Mapping.ChatMaping
{
    public class ChatOfflineMessageMapping : EntityTypeConfiguration<ChatOfflineMessageDetails>
    {

        public ChatOfflineMessageMapping()
        {
            this.HasKey(t => t.Id);

            this.ToTable("ChatOfflineMessages");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ImageUrl).HasColumnName("ImageUrl");
            this.Property(t => t.RecieverId).HasColumnName("RecieverId");
            this.Property(t => t.SenderId).HasColumnName("SenderId");
            this.Property(t => t.TextMessage).HasColumnName("TextMessage");
            this.Property(t => t.VideoUrl).HasColumnName("VideoUrl");
        }
    }
}
