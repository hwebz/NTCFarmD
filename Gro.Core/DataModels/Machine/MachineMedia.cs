namespace Gro.Core.DataModels.Machine
{
    public abstract class MachineMedia
    {
        public virtual string FileName { get; set; }
        public virtual string Name { get; set; }
        public virtual string Id { get; set; }

        public virtual string Url { get; set; }
    }
}
