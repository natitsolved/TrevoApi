using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trevo.API.Models
{

    public class UserInfoViewModel
    {

        public string Name { get; set; }

        public int IsVerified { get; set; }

        public string Email { get; set; }

        public long UserID { get; set; }


        public string ImagePath { get; set; }

        public string NativeLangugae { get; set; }

        public List<long> FavMomentList { get; set; }
        public string LearningLanguage { get; set; }

      

    }
}