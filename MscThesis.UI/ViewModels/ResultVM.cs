using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Core.Formats;
using MscThesis.Runner;
using MscThesis.Runner.Results;
using MscThesis.Runner.Specification;
using MscThesis.UI.Loading;
using MscThesis.UI.Models;
using System.Threading;

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

        [ObservableProperty]
        bool isRunning;

        private TestProvider _provider;
        private ITest<InstanceFormat> _test;

        public TestProvider Provider => _provider;
        public ITest<InstanceFormat> Test => _test;

        public ResultVM(Settings settings)
        {
            _provider = new TestProvider(settings);
        }

        public void BuildTest()
        {
            try
            {
                if (specification != null)
                {
                    _test = _provider.Run(specification);
                }
                else
                {
                    var content = File.ReadAllText(resultsFilePath);
                    var loader = new Loader(content, _provider);
                    _test = loader.Test;
                    Specification = loader.Specification;
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        public async Task RunTest(CancellationToken cancellationToken)
        {
            IsRunning = true;
            await _test.Execute(cancellationToken);
            IsRunning = false;
        }

    }
}
