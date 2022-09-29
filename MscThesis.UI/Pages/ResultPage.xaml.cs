using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Pages;

public partial class ResultPage : ContentPage
{
	private ResultVM _vm;

	public ResultPage(ResultVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_vm = vm;
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

		var result = _vm.Run();

        var label = new Label
        {
            BindingContext = result.Fittest
        };
        label.SetBinding(Label.TextProperty, "Value.Fitness", stringFormat: "Best fitness: {0}");
        Layout.Add(label);

		var optimizerNames = result.GetOptimizerNames();
		foreach (var optimizerName in optimizerNames)
		{
            var itemProps = result.GetItemProperties(optimizerName);
            foreach (var itemProp in itemProps)
            {
                var propName = itemProp.ToString();
                var observable = result.GetItemValue(optimizerName, itemProp);
                var itemLabel = new Label
                {
                    BindingContext = observable
                };
                itemLabel.SetBinding(Label.TextProperty, "Value", stringFormat: $"{propName}: {{0}}");
                Layout.Add(itemLabel);
            }

            var seriesProps = result.GetSeriesProperties(optimizerName);
            foreach (var seriesProp in seriesProps)
            {
                var propName = seriesProp.ToString();
                var values = result.GetSeriesValues(optimizerName, seriesProp);
                Layout.Add(new Label
                {
                    Text = $"{propName}:"
                });
                Layout.Add(new CartesianChart
                {
                    Series = new List<ISeries> {
                        new LineSeries<double>
                        {
                            Values = values,
                            Fill = null
                        }
                    },
                    HeightRequest = 400,
                    WidthRequest = 600
                });
            }

            var histogramProps = result.GetHistogramProperties(optimizerName);
            foreach (var prop in histogramProps)
            {
                var propName = prop.ToString();
                var values = result.GetHistogramValues(optimizerName, prop);
                Layout.Add(new Label
                {
                    Text = $"{propName}:"
                });
                Layout.Add(new CartesianChart
                {
                    Series = new List<ISeries> {
                        new ColumnSeries<double>
                        {
                            Values = values,
                            Fill = null
                        }
                    },
                    HeightRequest = 400,
                    WidthRequest = 600
                });
            }
        }

		await result.Execute();
    }

}