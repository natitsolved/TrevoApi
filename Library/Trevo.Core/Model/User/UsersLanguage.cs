using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trevo.Core.Model.User
{
   public class UsersLanguage :BaseEntity
    {
        public long UsersLanguageId { get; set; }

        public long User_Id { get; set; }

        public long Learning_LanguageId { get; set; }
        public long Native_LanguageId { get; set; }
    }
}
