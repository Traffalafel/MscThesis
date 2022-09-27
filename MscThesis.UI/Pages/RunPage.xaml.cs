using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Pages;

public partial class RunPage : ContentPage
{
	private RunVM _vm;

	public RunPage(RunVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_vm = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
		_vm.Run();
    }
}