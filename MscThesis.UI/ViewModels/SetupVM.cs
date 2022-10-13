using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{

    public class SetupVM
    {
        private string _problemName = string.Empty;

        private TestProvider _runner { get; set; }

        public SetupVM()
        {
            _runner = new TestProvider();
        }

        public string ProblemName {
            get => _problemName;
            set
            {
                _problemName = value;
                
                var optimizerNamesNew = _runner.GetAlgorithmNames(_problemName);
                PossibleAlgorithmNames.Clear();
                foreach (var optimizerName in optimizerNamesNew)
                {
                    PossibleAlgorithmNames.Add(optimizerName);
                }

                var terminationNamesNew = _runner.GetTerminationNames(_problemName);
                PossibleTerminationNames.Clear();
                foreach (var name in terminationNamesNew)
                {
                    PossibleTerminationNames.Add(name);
                }

                var newParameters = _runner.GetProblemParameters(_problemName);
                var vms = newParameters.Select(vm => new ParameterVM
                {
                    Name = vm.ToString()
                });

                ProblemParameters.Clear();
                foreach (var vm in vms)
                {
                    ProblemParameters.Add(vm);
                }
            } 
        }

        public string NumRuns { get; set; } = "1";

        public string ProblemSizeStart { get; set; }
        public string ProblemSizeStop { get; set; }
        public string ProblemSizeStep { get; set; }

        public string MaxParallelization { get; set; } = "1";
        public ObservableCollection<ParameterVM> ProblemParameters { get; set; } = new();
        public ObservableCollection<TerminationSetupVM> Terminations { get; set; } = new();
        public ObservableCollection<OptimizerSetupVM> Optimizers { get; set; } = new();

        public List<string> PossibleProblemNames => _runner.GetProblemNames();
        public ObservableCollection<string> PossibleAlgorithmNames { get; set; } = new();
        public ObservableCollection<string> PossibleTerminationNames { get; set; } = new();

        public void AddOptimizer()
        {
            var newOptimizer = new OptimizerSetupVM(_runner);
            Optimizers.Add(newOptimizer);
        }

        public void AddTerminationCriterion()
        {
            var newCriterion = new TerminationSetupVM(_runner);
            Terminations.Add(newCriterion);
        }

        public TestSpecification ToSpecification()
        {
            if (string.IsNullOrWhiteSpace(_problemName))
            {
                throw new Exception();
            }
            return new TestSpecification
            {
                NumRuns = Convert.ToInt32(NumRuns),
                Optimizers = Optimizers.Select(o => o.ToSpecification()).ToList(),
                MaxParallelization = Convert.ToDouble(MaxParallelization),
                Problem = new ProblemSpecification
                {
                    Name = _problemName,
                    Parameters = Utils.ToSpecification(ProblemParameters)
                },
                ProblemSizes = ParseProblemSizes(ProblemSizeStart, ProblemSizeStop, ProblemSizeStep).ToList(),
                Terminations = Terminations.Select(t => t.ToSpecification()).ToList()
            };
        }

        private IEnumerable<int> ParseProblemSizes(string startStr, string stopStr, string stepStr)
        {
            var start = Convert.ToInt32(startStr);
            var stop = Convert.ToInt32(stopStr);
            var step = Convert.ToInt32(stepStr);

            if (start <= 0 || stop <= 0)
            {
                throw new Exception("Start and stop must both be strictly positive.");
            }
            if (stop < start)
            {
                throw new Exception("Start must be lower than stop.");
            }
            if (step < 0)
            {
                throw new Exception("Cannot have negative step size.");
            }
            if (step == 0 && start != stop)
            {
                throw new Exception("Start and stop must be equal for step size zero.");
            }

            if (step == 0)
            {
                yield return start;
                yield break;
            }

            var c = start;
            do
            {
                yield return c;
                c += step;
            }
            while (c <= stop);
        }

    }

}
