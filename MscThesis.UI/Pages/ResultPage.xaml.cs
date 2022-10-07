using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using MscThesis.Runner.Results;
using MscThesis.UI.ViewModels;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace MscThesis.UI.Pages;

public partial class ResultPage : ContentPage
{
	private ResultVM _vm;
    private CancellationTokenSource _cancellationTokenSource;

	public ResultPage(ResultVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
		_vm = vm;
	}

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
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

        var propertyGroups = result.Series.GroupBy(series => series.Property);
        foreach (var propertyGroup in propertyGroups)
        {
            var propName = propertyGroup.Key.ToString();

            var optimizerGroups = propertyGroup.GroupBy(group => group.OptimizerName);
            var series = BuildSeries(optimizerGroups);

            Layout.Add(new Label
            {
                Text = $"{propName}:"
            });
            var chart = new CartesianChart
            {
                Series = series,
                XAxes = new List<Axis>
                {
                    new Axis
                    {
                        Name = propertyGroup.First().XAxis.ToString(),
                        NamePaint = new SolidColorPaint { Color = SKColors.White },
                        NameTextSize = 20
                    }
                },
                YAxes = new List<Axis>
                {
                    new Axis
                    {
                        Name = propertyGroup.Key.ToString(),
                        NamePaint = new SolidColorPaint { Color = SKColors.White },
                        NameTextSize = 20
                    }
                },
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom,
                LegendFontSize = 12,
                LegendTextBrush = Color.FromRgb(255, 255, 255),
                LegendBackground = Color.FromRgb(0, 0, 0),
                LegendOrientation = LiveChartsCore.Measure.LegendOrientation.Vertical,
                HeightRequest = 500,
                WidthRequest = 700
            };
            Layout.Add(chart);

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

        try
        {
            _cancellationTokenSource = new CancellationTokenSource();
    		await result.Execute(_cancellationTokenSource.Token);
        }
        catch (Exception e)
        {
            ;
        }
    }

    private List<LineSeries<(double,double)>> BuildSeries(IEnumerable<IGrouping<string, SeriesResult>> groups)
    {
        return groups.Select(group =>
        {
            if (group.Count() > 1)
            {
                // Points are not connected
                return new LineSeries<(double, double)>
                {
                    Values = group.SelectMany(x => x.Points),
                    Mapping = (value, point) =>
                    {
                        point.PrimaryValue = value.Item2;
                        point.SecondaryValue = value.Item1;
                    },
                    Stroke = new SolidColorPaint { Color = SKColors.Transparent },
                    Fill = null,
                    Name = group.Key
                };
            }
            else
            {
                // Points are connected
                return new LineSeries<(double, double)>
                {
                    Values = group.First().Points,
                    Mapping = (value, point) =>
                    {
                        point.PrimaryValue = value.Item2;
                        point.SecondaryValue = value.Item1;
                    },
                    Fill = null,
                    Name = group.Key
                };
            }
        }).ToList();
    }

}