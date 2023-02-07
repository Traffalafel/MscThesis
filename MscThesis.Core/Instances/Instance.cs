
namespace MscThesis.Core.Formats
{
    public abstract class Instance
    {
        public abstract int Size { get; }
        public override abstract string ToString();
        public double? Fitness { get; set; }
    }
}
