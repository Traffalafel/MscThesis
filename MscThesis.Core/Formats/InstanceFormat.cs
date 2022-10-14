using System;

namespace MscThesis.Core.Formats
{
    public abstract class InstanceFormat
    {
        public abstract int Size { get; }
        public override abstract string ToString();
    }
}
