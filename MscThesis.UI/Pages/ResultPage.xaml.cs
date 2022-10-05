using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using MscThesis.Runner.Results;
using MscThesis.UI.ViewModels;
using SkiaSharp;

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

        foreach (var item in result.Items)
        {
            var propName = item.Property.ToString();
            var itemLabel = new Label
            {
                BindingContext = item.Observable
            };
            itemLabel.SetBinding(Label.TextProperty, "Value", stringFormat: $"{propName}: {{0}}");
            Layout.Add(itemLabel);
        }

        var groups = result.Series.GroupBy(series => series.Property);
        foreach (var group in groups)
        {
            var propName = group.Key.ToString();

            var first = group.First();
            Layout.Add(new Label
            {
                Text = $"{propName}:"
            });
            Layout.Add(new CartesianChart
            {
                Series = CreateSeries(group),
                XAxes = new List<Axis>
                {
                    new Axis
                    {
                        Name = first.XAxis.ToString(),
                        NamePaint = new SolidColorPaint { Color = SKColors.White },
                        NameTextSize = 20
                    }
                },
                YAxes = new List<Axis>
                {
                    new Axis
                    {
                        Name = group.Key.ToString(),
                        NamePaint = new SolidColorPaint { Color = SKColors.White },
                        NameTextSize = 20
                    }
                },
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom,
                LegendFontSize = 12,
                LegendTextBrush = Color.FromRgb(255,255,255),
                LegendBackground = Color.FromRgb(0,0,0),
                LegendOrientation = LiveChartsCore.Measure.LegendOrientation.Vertical,
                HeightRequest = 500,
                WidthRequest = 700
            }); ;

        }

        //foreach (var histogram in result.Histograms)
        //{
        //    var propName = histogram.Property.ToString();
        //    Layout.Add(new Label
        //    {
        //        Text = $"{propName}:"
        //    });
        //    Layout.Add(new CartesianChart
        //    {
        //        Series = new List<ISeries> {
        //                new ColumnSeries<double>
        //                {
        //                    Values = histogram.Values,
        //                    Fill = null
        //                }
        //            },
        //        HeightRequest = 400,
        //        WidthRequest = 600
        //    });
        //}

		await result.Execute();
    }

    private List<LineSeries<(double,double)>> CreateSeries(IEnumerable<SeriesResult> results)
    {
        return results.Select(series => new LineSeries<(double, double)>
        {
            Values = series.Points,
            Mapping = (value, point) =>
            {
                point.PrimaryValue = value.Item2;
                point.SecondaryValue = value.Item1;
            },
            Fill = null,
            Name = series.OptimizerName
        }).ToList();
    }

}