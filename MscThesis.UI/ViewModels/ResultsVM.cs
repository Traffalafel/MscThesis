using CommunityToolkit.Mvvm.ComponentModel;
using MscThesis.Core.Formats;
using MscThesis.Runner.Results;

namespace MscThesis.UI.ViewModels
{
    public partial class ResultsVM : ObservableObject
    {
        [ObservableProperty]
        IResult<InstanceFormat> result;

        public ResultsVM()
        {

        }
    }
}
