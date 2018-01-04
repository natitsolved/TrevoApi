using System;

namespace Trevo.Core.Model.User
{
    public  class UserWithCountryIcon :BaseEntity
    {
        public long User_Id { get; set; }
        public string Name { get; set; }
        public string Email_Id { get; set; }
        public string Password { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string DeviceId { get; set; }
        public string TrevoId { get; set; }

        public string Self_Introduction { get; set; }

        public long Country_Id { get; set; }
        public string Address { get; set; }

        public string QR_Code { get; set; }

        public string Interests { get; set; }

        public string TravelDestination_CId { get; set; }

        public long LagLevel_ID { get; set; }

        public string PasswordHash { get; set; }

        public int IsVerified { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Flag_Icon { get; set; }

        public long Learning_LanguageId { get; set; }

        public long Native_LanguageId { get; set; }
        public string ExternalAuthType { get; set; }

        public string ExternalAuthUserId { get; set; }
    }
}
