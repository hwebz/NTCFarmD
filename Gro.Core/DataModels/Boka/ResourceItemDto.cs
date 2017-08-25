
namespace Gro.Core.DataModels.Boka
{
    public class ResourceItemDto
    {
        public ResourceItemDto()
        {
            this.Url = string.Empty;
            this.RowType = new ResourceGroupTypeDto();
            this.Name = string.Empty;
            this.Value = string.Empty;
            this.RegNoMandatory = false;
            this.M3Id = string.Empty;
        }

        public string Url { get; set; }
        public string Value { get; set; }
        public bool RegNoMandatory { get; set; }
        public string M3Id { get; set; }
        public ResourceGroupTypeDto RowType { get; set; }
        public string Name { get; set; }
        public string Display => this.Name + " - " + this.RowType.Name;
    }
}
