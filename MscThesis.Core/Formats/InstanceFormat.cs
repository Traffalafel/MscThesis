using System;

namespace MscThesis.Core.Formats
{
    public abstract class InstanceFormat
    {
        public abstract int GetSize();
        public override abstract string ToString();
    }
}
