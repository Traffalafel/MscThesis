using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner;
using MscThesis.Runner.Results;
using MscThesis.Runner.Specification;

namespace MscThesis.UI.ViewModels
{
    [QueryProperty(nameof(Specification), nameof(Specification))]
    public partial class ResultVM : ObservableObject
    {
        [ObservableProperty]
        TestSpecification specification;

        [ObservableProperty]
        double bestFitness;
        
        private TestRunner _runner;

        public ResultVM(TestRunner runner)
        {
            _runner = runner;
        }

        public IResult<InstanceFormat> Run()
        {
            return _runner.Run(specification);
        }

        public void UpdateBestFitness(Individual<InstanceFormat> fittest)
        {
            BestFitness = fittest.Fitness.Value;
        }

    }
}
