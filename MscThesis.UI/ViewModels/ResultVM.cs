using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Core.Formats;
using MscThesis.Runner;
using MscThesis.Runner.Results;
using MscThesis.Runner.Specification;
using MscThesis.UI.Models;

namespace MscThesis.UI.ViewModels
{
    [QueryProperty(nameof(Specification), nameof(Specification))]
    [QueryProperty(nameof(ResultsFilePath), nameof(ResultsFilePath))]
    public partial class ResultVM : ObservableObject
    {
        [ObservableProperty]
        TestSpecification specification;

        [ObservableProperty]
        string resultsFilePath;

        [ObservableProperty]
        double bestFitness;
        
        private TestProvider _runner;

        public ResultVM(TestProvider runner)
        {
            _runner = runner;
        }

        public ITest<InstanceFormat> BuildTest()
        {
            if (specification != null)
            {
                return _runner.Run(specification);
            }

            if (resultsFilePath != null)
            {
                var content = File.ReadAllText(resultsFilePath);
                return new LoadedTest(content);
            }

            throw new Exception();
        }

    }
}
