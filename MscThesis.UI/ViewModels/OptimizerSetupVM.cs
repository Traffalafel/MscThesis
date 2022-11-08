using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{
    public partial class OptimizerSetupVM : ObservableObject
    {
        [ObservableProperty]
        bool customSeed = false;

        [ObservableProperty]
        bool parametersExist = false;

        private TestProvider _runner;
        private string _algorithm = string.Empty;

        public OptimizerSetupVM(TestProvider runner)
        {
            _runner = runner;
        }

        public string Seed { get; set; }
        public string Name { get; set; }
        public ObservableCollection<ExpressionParameterVM> Parameters { get; set; } = new();
        public string Algorithm
        {
            get => _algorithm;
            set
            {
                _algorithm = value;
                var paramsNew = _runner.GetAlgorithmParameters(value);
                var vms = paramsNew.Select(p => new ExpressionParameterVM
                {
                    Name = p.ToString()
                });
                Parameters.Clear();
                foreach (var vm in vms)
                {
                    Parameters.Add(vm);
                }
                ParametersExist = paramsNew.Count() > 0;
            }
        }

        private string GetName()
        {
            var paramStrings = Parameters.Select(vm => $"{vm.Name}:{vm.Value}");
            return $"{Algorithm}_{string.Join('_', paramStrings)}";
        }

        public OptimizerSpecification ToSpecification()
        {
            if (string.IsNullOrWhiteSpace(_algorithm))
            {
                throw new Exception();
            }
            return new OptimizerSpecification
            {
                Name = GetName(),
                Algorithm = _algorithm,
                Parameters = Utils.ToSpecification(Parameters)
            };
        }

    }

}
