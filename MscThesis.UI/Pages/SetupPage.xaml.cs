using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Pages;

public partial class SetupPage : ContentPage
{
    public SetupVM VM { get; set; }

	public SetupPage()
	{
		VM = new SetupVM();

		BindingContext = VM;

		InitializeComponent();
	}

    public void AddOptimizer(object sender, EventArgs e)
    {
        VM.AddOptimizer();
    }

    public void RemoveOptimizer(object sender, EventArgs e)
    {
		var button = (Button)sender;
		var buttonVM = (OptimizerSetupVM)button.BindingContext;
		VM.Optimizers.Remove(buttonVM);
    }

	public void AddTerminationCriterion(object sender, EventArgs e)
	{
        VM.AddTerminationCriterion();
    }

    public void RemoveTerminationCriterion(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var buttonVM = (TerminationSetupVM)button.BindingContext;
        VM.Terminations.Remove(buttonVM);
    }

    public void Run(object sender, EventArgs e)
    {
        var specification = VM.ToSpecification();
        Shell.Current.GoToAsync(nameof(ResultPage), new Dictionary<string, object>
        {
            ["Specification"] = specification
        });
	}
}