using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Core.Formats;
using MscThesis.Framework;
using MscThesis.Framework.Tests;
using MscThesis.Framework.Specification;
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
        private ITest _test;

        public TestProvider Provider => _provider;
        public ITest Test => _test;

        public ResultVM(string tspLibPath)
        {
            _provider = new TestProvider(tspLibPath);
        }

        public void BuildTest()
        {
            try
            {
                if (specification != null)
                {
                    _test = _provider.Build(specification);
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

        public void RunTest(CancellationToken cancellationToken)
        {
            IsRunning = true;
            Task.Run(async () =>
            {
                await _test.Execute(cancellationToken);
                IsRunning = false;
            });
        }

    }
}
