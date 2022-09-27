using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using MscThesis.UI.Pages;

namespace MscThesis.UI.ViewModels
{
    [QueryProperty(nameof(Specification), nameof(Specification))]
    public partial class RunVM : ObservableObject
    {
        [ObservableProperty]
        TestSpecification specification;

        private TestRunner _runner;

        public RunVM(TestRunner runner)
        {
            _runner = runner;
        }

        public void Run()
        {
            var results = _runner.Run(specification);

            // TODO: stream results

            Shell.Current.GoToAsync(nameof(ResultPage), new Dictionary<string, object>
            {
                ["Results"] = results
            });
        }
    }
}
