using MscThesis.Core;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{
    public static class Utils
    {
        public static IDictionary<Parameter, string> ToSpecification(ObservableCollection<ExpressionParameterVM> vms)
        {
            try
            {
                return vms
                    .GroupBy(vm => vm.Parameter)
                    .ToDictionary(x => x.Key, x => x.First().Value);
            }
            catch (Exception e)
            {
                return new Dictionary<Parameter, string>();
            }
        }

        public static IDictionary<Parameter, string> ToSpecification(ObservableCollection<OptionParameterVM> vms)
        {
            try
            {
                return vms
                    .GroupBy(vm => vm.Parameter)
                    .ToDictionary(x => x.Key, x => x.First().Value);
            }
            catch (Exception e)
            {
                return new Dictionary<Parameter, string>();
            }
        }

        public static IDictionary<Parameter, string> Join(IDictionary<Parameter, string> dic1, IDictionary<Parameter, string> dic2)
        {
            var output = new Dictionary<Parameter, string>();
            foreach (var kv in dic1)
            {
                output.Add(kv.Key, kv.Value);
            }
            foreach (var kv in dic2)
            {
                output.Add(kv.Key, kv.Value);
            }
            return output;
        }
    }

}
