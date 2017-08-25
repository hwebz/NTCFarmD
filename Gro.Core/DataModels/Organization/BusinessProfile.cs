namespace Gro.Core.DataModels.Organization
{
    public class BusinessProfile
    {
        public int CustomerId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public ProfileRow[] Rows { get; set; }
    }
}
