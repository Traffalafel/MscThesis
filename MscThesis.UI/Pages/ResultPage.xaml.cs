using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Pages;

public partial class ResultPage : ContentPage
{
	private ResultsVM _vm;

	public ResultPage(ResultsVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_vm = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
		;
    }
}