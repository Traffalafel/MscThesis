using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{
    public class TerminationSetupViewModel
    {
        public string Name { get; set; } = string.Empty;
    }

    public class OptimizerSetupViewModel
    {
        public int? Seed { get; set; } = null;
        public string Name { get; set; } = string.Empty;
        public string Algorithm { get; set; } = string.Empty;
        public ObservableCollection<TerminationSetupViewModel> TerminationCriteria { get; set; } = new ObservableCollection<TerminationSetupViewModel>();

        public void AddTerminationCriterion()
        {
            var newCriterion = new TerminationSetupViewModel();
            TerminationCriteria.Add(newCriterion);
        }
    }

    public class SetupPageViewModel
    {
        private string _problemName = string.Empty;
        private List<string> _possibleAlgorithmNames = new List<string>();

        private TestRunner _runner { get; set; } = new TestRunner();

        public string ProblemName { 
            get 
            {
                return _problemName;
            } 
            set
            {
                _problemName = value;
                _possibleAlgorithmNames = _runner.GetAlgorithmNames(value);
            } 
        }

        public List<string> ProblemNames => _runner.GetProblemNames();
        public List<string> OptimizerNames => _possibleAlgorithmNames;

        public int NumRuns { get; set; } = 1;
        public ObservableCollection<OptimizerSetupViewModel> Optimizers { get; set; } = new ObservableCollection<OptimizerSetupViewModel>();
    }
}
