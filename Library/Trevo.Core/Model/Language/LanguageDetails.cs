using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trevo.Core.Model.Language
{
   public class LanguageDetails :BaseEntity
    {
        public long Language_Id { get; set; }

        public string Name { get; set; }
        public string ImagePath { get; set; }

        public string Abbreviation { get; set; }
    }
}
