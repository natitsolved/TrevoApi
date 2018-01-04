namespace Trevo.Core.Model.Country
{
    public class Country : BaseEntity
    {
        public long Country_Id { get; set; }

        public string Name { get; set; }

        public string Flag_Icon { get; set; }

        public string ImagePath { get; set; }
    }
}
