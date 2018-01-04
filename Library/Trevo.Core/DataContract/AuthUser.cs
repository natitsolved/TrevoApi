using System;

namespace Trevo.Core.DTO
{
    public partial class AuthUser : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int UserTypeId { get; set; }
        public string Info { get; set; }
        public int IndividualId { get; set; }
        public int UserId { get; set; }
        public Guid AccessToken { get; set; }
    }
}
