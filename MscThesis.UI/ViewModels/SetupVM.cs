using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{

    public partial class SetupVM : ObservableObject
    {
        [ObservableProperty]
        bool customProblemSizesAllowed = false;

        [ObservableProperty]
        bool parallelizationEnabled = false;

        [ObservableProperty]
        bool multipleSizes = false;

        [ObservableProperty]
        string problemInformationName = string.Empty;

        [ObservableProperty]
        string problemInformationDescription = string.Empty;

        [ObservableProperty]
        bool expressionParamsExist = false;
        [ObservableProperty]
        bool optionParamsExist = false;
        [ObservableProperty]
        bool anyOptimizersExist = false;
        [ObservableProperty]
        bool anyTerminationsExist = false;

        private string _problemName = string.Empty;

        private TestProvider _provider { get; set; }

        public SetupVM(Settings settings)
        {
            _provider = new TestProvider(settings);
        }

        public string ProblemName {
            get => _problemName;
            set
            {
                _problemName = value;
                
                var optimizerNamesNew = _provider.GetAlgorithmNames(_problemName);
                PossibleAlgorithmNames.Clear();
                foreach (var optimizerName in optimizerNamesNew)
                {
                    PossibleAlgorithmNames.Add(optimizerName);
                }

                var terminationNamesNew = _provider.GetTerminationNames(_problemName);
                PossibleTerminationNames.Clear();
                foreach (var name in terminationNamesNew)
                {
                    PossibleTerminationNames.Add(name);
                }

                try
                {
                    var problemSpec = CreateProblemSpec();
                    var definition = _provider.GetProblemDefinition(problemSpec);

                    CustomProblemSizesAllowed = definition.CustomSizesAllowed;

                    var expressionVMs = definition.ExpressionParameters.Select(vm => new ExpressionParameterVM
                    {
                        Name = vm.ToString()
                    });
                    ProblemExpressionParameters.Clear();
                    foreach (var vm in expressionVMs)
                    {
                        ProblemExpressionParameters.Add(vm);
                    }
                    ExpressionParamsExist = expressionVMs.Count() > 0;

                    var optionVMs = definition.OptionParameters.Select(kv =>
                    {
                        return new OptionParameterVM(kv.Key, string.Join(',', kv.Value));
                    });
                    OptionParamsExist = optionVMs.Count() > 0;
                    
                    ProblemOptionParameters.Clear();
                    foreach (var vm in optionVMs)
                    {
                        ProblemOptionParameters.Add(vm);
                    }

                    ReloadProblemInformation();
                }
                catch (Exception ex)
                {
                    ;
                }
            } 
        }

        public void ReloadProblemInformation()
        {
            var spec = CreateProblemSpec();
            var info = _provider.GetProblemInformation(spec);
            ProblemInformationName = info.Name;
            ProblemInformationDescription = info.Description;
        }

        public string NumRuns { get; set; } = "1";

        public string ProblemSizeStart { get; set; }
        public string ProblemSizeStop { get; set; }
        public string ProblemSizeStep { get; set; }

        public string MaxParallelization { get; set; } = "1";

        public ObservableCollection<ExpressionParameterVM> ProblemExpressionParameters { get; set; } = new();
        public ObservableCollection<OptionParameterVM> ProblemOptionParameters { get; set; } = new();

        public ObservableCollection<TerminationSetupVM> Terminations { get; set; } = new();
        public ObservableCollection<OptimizerSetupVM> Optimizers { get; set; } = new();

        public List<string> PossibleProblemNames => _provider.GetProblemNames();
        public ObservableCollection<string> PossibleAlgorithmNames { get; set; } = new();
        public ObservableCollection<string> PossibleTerminationNames { get; set; } = new();

        public void AddOptimizer()
        {
            var newOptimizer = new OptimizerSetupVM(_provider);
            Optimizers.Add(newOptimizer);
            AnyOptimizersExist = true;
        }

        public void AddTerminationCriterion()
        {
            var newCriterion = new TerminationSetupVM(_provider);
            Terminations.Add(newCriterion);
            AnyTerminationsExist = true;
        }

        private ProblemSpecification CreateProblemSpec()
        {
            return new ProblemSpecification
            {
                Name = _problemName,
                Parameters = Utils.Join(Utils.ToSpecification(ProblemExpressionParameters),
                                        Utils.ToSpecification(ProblemOptionParameters))          
            };
        }

        public TestSpecification ToSpecification()
        {
            if (string.IsNullOrWhiteSpace(_problemName))
            {
                throw new Exception();
            }

            var problemSizes = new List<int>();
            if (CustomProblemSizesAllowed)
            {
                if (MultipleSizes)
                {
                    problemSizes = ParseProblemSizes(ProblemSizeStart, ProblemSizeStop, ProblemSizeStep).ToList();
                }
                else
                {
                    problemSizes = new List<int> { Convert.ToInt32(ProblemSizeStart) };
                }
            }

            return new TestSpecification
            {
                NumRuns = Convert.ToInt32(NumRuns),
                Optimizers = Optimizers.Select(o => o.ToSpecification()).ToList(),
                MaxParallelization = Convert.ToDouble(MaxParallelization),
                Problem = CreateProblemSpec(),
                ProblemSizes = problemSizes,
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
