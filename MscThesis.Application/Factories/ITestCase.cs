using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner.Results;

namespace MscThesis.Runner.Factories
{
    public interface ITestCase<out T> where T : InstanceFormat
    {
        public ITest<T> CreateRun(int problemSize, bool saveSeries, VariableSpecification variableSpecification);
    }
}
