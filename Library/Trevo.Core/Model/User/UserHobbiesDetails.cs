namespace Trevo.Core.Model.User
{
    public class UserHobbiesDetails :BaseEntity
    {
        public long UserHobbiesId { get; set; }

        public long User_Id { get; set; }

        public long HobbiesId { get; set; }
    }
}
