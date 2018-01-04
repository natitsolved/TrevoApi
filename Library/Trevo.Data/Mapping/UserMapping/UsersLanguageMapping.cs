using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trevo.Core.Model.User;

namespace Trevo.Data.Mapping.UserMapping
{
   public class UsersLanguageMapping : EntityTypeConfiguration<UsersLanguage>
    {
        public UsersLanguageMapping()
        {
            this.HasKey(t => t.UsersLanguageId);

            //mapping to the table and the properties

            this.ToTable("UsersLanguage");

            this.Property(t => t.UsersLanguageId).HasColumnName("UsersLanguageId");
            this.Property(t => t.Learning_LanguageId).HasColumnName("Learning_LanguageId");
            this.Property(t => t.User_Id).HasColumnName("User_Id");
            this.Property(t => t.Native_LanguageId).HasColumnName("Native_LanguageId");
        }
    }
}
