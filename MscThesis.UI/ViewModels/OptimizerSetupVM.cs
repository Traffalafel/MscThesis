using MscThesis.Runner;
using MscThesis.Runner.Specification;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{
    public class OptimizerSetupVM
    {
        private TestProvider _runner;
        private string _algorithm = string.Empty;

        public OptimizerSetupVM(TestProvider runner)
        {
            _runner = runner;
        }

        public string Seed { get; set; }
        public string Name { get; set; }
        public ObservableCollection<ParameterVM> Parameters { get; set; } = new();
        public string Algorithm
        {
            get => _algorithm;
            set
            {
                _algorithm = value;
                var paramsNew = _runner.GetAlgorithmParameters(value);
                var vms = paramsNew.Select(p => new ParameterVM
                {
                    Name = p.ToString()
                });
                Parameters.Clear();
                foreach (var vm in vms)
                {
                    Parameters.Add(vm);
                }
            }
        }

        public OptimizerSpecification ToSpecification()
        {
            if (string.IsNullOrWhiteSpace(_algorithm))
            {
                throw new Exception();
            }
            return new OptimizerSpecification
            {
                Seed = string.IsNullOrWhiteSpace(Seed) ? null : Convert.ToInt32(Seed),
                Name = Name,
                Algorithm = _algorithm,
                Parameters = Utils.ToSpecification(Parameters)
            };
        }

    }

}
