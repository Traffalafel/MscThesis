using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Extensions.Configuration;
using MscThesis.Core.Formats;
using MscThesis.Runner.Results;
using MscThesis.UI.Loading;
using MscThesis.UI.ViewModels;
using SkiaSharp;

namespace MscThesis.UI.Pages;

public partial class ResultPage : ContentPage
{
	private ResultVM _vm;
    private CancellationTokenSource _cancellationTokenSource;

    private ITest<InstanceFormat> _test;
    private string _resultsDir;

	public ResultPage(ResultVM vm, IConfiguration config)
	{
		InitializeComponent();
		BindingContext = vm;
		_vm = vm;

        _resultsDir = config["ResultsDirectory"];
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

		_test = _vm.BuildTest();

        var label = new Label
        {
            BindingContext = _test.Fittest
        };
        label.SetBinding(Label.TextProperty, "Value.Fitness", stringFormat: "Best fitness: {0}");
        Layout.Add(label);

        foreach (var item in _test.Items)
        {
            var propName = item.Property.ToString();
            var itemLabel = new Label
            {
                BindingContext = item.Observable
            };
            itemLabel.SetBinding(Label.TextProperty, "Value", stringFormat: $"{propName}: {{0}}");
            Layout.Add(itemLabel);
        }

        var propertyGroups = _test.Series.GroupBy(series => series.Property);
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
    		await _test.Execute(_cancellationTokenSource.Token);
        }
        catch (Exception e)
        {
            var msg = $"An exception was thrown with the message:\n{e.Message}";
            await DisplayAlert("Exception occured", msg, "Close");
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
                    Name = group.Key,
                    LineSmoothness = 0
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
                    Name = group.Key,
                    LineSmoothness = 0
                };
            }
        }).ToList();
    }

    public void Save(object sender, EventArgs e)
    {
        if (!Directory.Exists(_resultsDir))
        {
            try
            {
                Directory.CreateDirectory(_resultsDir);
            }
            catch
            {
                return;
            }
        }

        try
        {
            var content = ResultExporter.Export(_test);
            var fileName = DateTime.Now.ToString("dd MMM HHmmss");
            var filePath = Path.Combine(_resultsDir, $"{fileName}.txt");
            File.WriteAllText(filePath, content);

            var message = $"Saved to file \"{filePath}\"";
            DisplayAlert("", message, "Close");
        }
        catch
        {
            return;
        }
    }

}