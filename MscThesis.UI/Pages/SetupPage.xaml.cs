using MscThesis.UI.Behaviors;
using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Pages;

public partial class SetupPage : ContentPage
{
    public SetupVM _vm { get; set; }

	public SetupPage()
	{
        _vm = new SetupVM();

		BindingContext = _vm;

        NumericValidator.VM = _vm;
        ExpressionValidator.VM = _vm;
        NameValidator.VM = _vm;
        ParentBehavior.VM = _vm;

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
        try
        {
		    var button = (Button)sender;
		    var buttonVM = (OptimizerSetupVM)button.BindingContext;
		    _vm.Optimizers.Remove(buttonVM);
        }
        catch (Exception ex)
        {
            ;
        }
    }

	public void AddTerminationCriterion(object sender, EventArgs e)
	{
        _vm.AddTerminationCriterion();
    }

    public void RemoveTermination(object sender, EventArgs e)
    {
        try
        {
            var button = (Button)sender;
            var buttonVM = (TerminationSetupVM)button.BindingContext;
            _vm.Terminations.Remove(buttonVM);
        }
        catch (Exception ex)
        {
            ;
        }
    }

    public void Run(object sender, EventArgs e)
    {
        var specification = _vm.ToSpecification();
        Shell.Current.GoToAsync(nameof(ResultPage), new Dictionary<string, object>
        {
            ["Specification"] = specification
        });
	}
}