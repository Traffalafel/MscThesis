using MscThesis.Runner;
using MscThesis.Runner.Specification;
using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Pages;

public partial class SetupPage : ContentView
{
    public SetupPageViewModel VM { get; set; }

	public SetupPage()
	{
		VM = new SetupPageViewModel();

		BindingContext = VM;

		InitializeComponent();
	}

    public void AddOptimizer(object sender, EventArgs e)
    {
		var newOptimizer = new OptimizerSetupViewModel();
		VM.Optimizers.Add(newOptimizer);
    }

    public void RemoveOptimizer(object sender, EventArgs e)
    {
		var button = (Button)sender;
		var buttonVM = (OptimizerSetupViewModel)button.BindingContext;
		VM.Optimizers.Remove(buttonVM);
    }

	public void AddTerminationCriterion(object sender, EventArgs e)
	{
        var button = (Button)sender;
        var buttonVM = (OptimizerSetupViewModel)button.BindingContext;
        buttonVM.AddTerminationCriterion();
    }

    public void RemoveTerminationCriterion(object sender, EventArgs e)
    {

    }

    public void Run(object sender, EventArgs e)
    {
		;
	}
}