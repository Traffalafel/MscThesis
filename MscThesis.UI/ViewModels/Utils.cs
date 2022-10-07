using MscThesis.Core;
using System.Collections.ObjectModel;

namespace MscThesis.UI.ViewModels
{
    public static class Utils
    {
        public static IDictionary<Parameter, string> ToSpecification(ObservableCollection<ParameterVM> vms)
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
    }

}
