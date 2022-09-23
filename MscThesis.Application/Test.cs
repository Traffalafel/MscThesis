using MscThesis.Core.Formats;
using MscThesis.Runner.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MscThesis.Runner
{
    public interface Test<out T> where T : InstanceFormat
    {
        public IResult<T> Run(int size);
    }
}
