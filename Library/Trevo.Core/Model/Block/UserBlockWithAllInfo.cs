namespace Trevo.Core.Model.Block
{
    public class UserBlockWithAllInfo :BaseEntity
    {
        public long BlockId { get; set; }

        public long BlockingUserId { get; set; }

        public long BlockedUserId { get; set; }

        public string BlockingTime { get; set; }

        public string Name { get; set; }

        public string Email_Id { get; set; }

        public long User_Id { get; set; }

        public string ImagePath { get; set; }

        public long Country_Id { get; set; }

        public long LagLevel_Id { get; set; }

        public string Flag_Icon { get; set; }

        public long learning_languageId { get; set; }

        public long native_languageId { get; set; }

        public string learningAbbreviation { get; set; }

        public string nativeAbbreviation { get; set; }
    }
}
