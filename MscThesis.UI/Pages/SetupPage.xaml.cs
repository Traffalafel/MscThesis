using Microsoft.Extensions.Configuration;
using MscThesis.Runner;
using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Pages;

public partial class SetupPage : ContentPage
{
    public SetupVM _vm { get; set; }

	public SetupPage(IConfiguration config)
	{
        var settings = new Settings
        {
            TSPLibDirectoryPath = config["TSPLibDirectoryPath"]
        };

        _vm = new SetupVM(settings);

		BindingContext = _vm;

		InitializeComponent();
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    public void AddOptimizer(object sender, EventArgs e)
    {
        _vm.AddOptimizer();
    }

    public void RemoveOptimizer(object sender, EventArgs e)
    {
		var button = (Button)sender;
		var buttonVM = (OptimizerSetupVM)button.BindingContext;
		_vm.Optimizers.Remove(buttonVM);
        if (!_vm.Optimizers.Any())
        {
            _vm.AnyOptimizersExist = false;
        }
    }

	public void AddTerminationCriterion(object sender, EventArgs e)
	{
        _vm.AddTerminationCriterion();
    }

    public void RemoveTermination(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var buttonVM = (TerminationSetupVM)button.BindingContext;
        _vm.Terminations.Remove(buttonVM);
        if (!_vm.Terminations.Any())
        {
            _vm.AnyTerminationsExist = false;
        }
    }

    public async void Run(object sender, EventArgs e)
    {
        try
        {
            var specification = _vm.ToSpecification();
            await Shell.Current.GoToAsync(nameof(ResultPage), new Dictionary<string, object>
            {
                ["Specification"] = specification
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Please fix any errors before proceeding.", "Close");
        }
	}
}