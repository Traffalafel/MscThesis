using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using MscThesis.Core;
using MscThesis.Core.Formats;
using MscThesis.Runner.Results;

namespace MscThesis.UI.Views;

public partial class TestCaseResultView : ContentView
{
	public TestCaseResultView(IEnumerable<IView> views)
	{
		InitializeComponent();
		foreach (var view in views)
		{
			Layout.Add(view);
		}
	}

	public static TestCaseResultView Create<T>(IResult<T> result, string testCase) where T : InstanceFormat
	{
		var views = new List<IView>();
		foreach (var property in result.GetItemProperties(testCase))
		{
			var value = result.GetItemValue(testCase, property);
			var label = new Label
						{
							Text = $"{GetName(property)}: {value}"
						};
			views.Add(label);
		}
		foreach (var property in result.GetSeriesProperties(testCase))
		{
			var values = result.GetSeriesValues(testCase, property);
			var label = new Label
			{
				Text = $"{GetName(property)}:"
			};
			ISeries[] series =
			{
				new LineSeries<double>
				{
					Values = values
				}
			};
			var chart = new CartesianChart
			{
				Series = series,
				HeightRequest = 300,
				WidthRequest = 500
			};
            views.Add(label);
            views.Add(chart);
        }

		return new TestCaseResultView(views);
	}

	public static string GetName(Property property)
	{
		return Enum.GetName(typeof(Property), property);
    }

}