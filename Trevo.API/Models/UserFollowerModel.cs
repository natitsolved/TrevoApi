using System;
using System.ComponentModel.DataAnnotations;

namespace Trevo.API.Models
{
    public class UserFollowerModel
    {
        [Required]
        public long FollowerUserId { get; set; }

        [Required]
        public long FollowingUserId { get; set; }

        public string FollowerName { get; set; }

        public string FollowingName { get; set; }

        public long UserFollowId { get; set; }
    }


    public class UserDetailsModel {

        [Required]
        public long User_Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Email_Id { get; set; }
        [MaxLength(255)]
        public string Password { get; set; }
        [MaxLength(255)]
        public string Dob { get; set; }
        [MaxLength(15)]
        public string Gender { get; set; }
        [MaxLength(255)]
        public string DeviceId { get; set; }
        [MaxLength(10)]
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

        public string ExternalAuthType { get; set; }

        public string ExternalAuthUserId { get; set; }
    }
}