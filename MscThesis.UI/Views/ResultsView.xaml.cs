using MscThesis.Core.Formats;
using MscThesis.Runner.Results;

namespace MscThesis.UI.Views;

public partial class ResultsView : ContentView
{
	public ResultsView(IEnumerable<IView> views)
	{
		InitializeComponent();
		foreach (var view in views)
		{
			Layout.Add(view);
		}
	}

	public static ResultsView Create<T>(IResult<T> results) where T : InstanceFormat
	{
		var testCases = results.GetOptimizerNames();

		var views = new List<IView>();
		foreach (var testCase in testCases)
		{
			var view = TestCaseResultView.Create(results, testCase);
			views.Add(view);
		}

		return new ResultsView(views);
	}
}