using MscThesis.Core.Formats;

namespace MscThesis.Core
{
    public interface Individual<out T> where T : InstanceFormat
    {
        public T Value { get; }
        public double? Fitness { get; set; }
        public int Size { get; }
        public string ToString();
    }
}
