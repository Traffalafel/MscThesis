using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Extensions.Configuration;
using MscThesis.Core.Formats;
using MscThesis.Runner.Results;
using MscThesis.UI.Loading;
using MscThesis.Runner;
using MscThesis.UI.ViewModels;
using SkiaSharp;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Maui;
using System.Collections.Generic;

namespace MscThesis.UI.Pages;

public partial class ResultPage : ContentPage
{
	private ResultVM _vm;
    private CancellationTokenSource _cancellationTokenSource;

    private string _resultsDir;

	public ResultPage(IConfiguration config)
	{
        var settings = new Settings
        {
            TSPLibDirectoryPath = config["TSPLibDirectoryPath"]
        };

        _vm = new ResultVM(settings);

        BindingContext = _vm;

        _resultsDir = config["ResultsDirectory"];
        
        InitializeComponent();
	}

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    private static int TitleSize = 22;
    private static int SubTitleSize = 18;
    private static Thickness BoxMargin => new Thickness(20, 10, 0, 10);
    private static Thickness BoxTitleMargin => new Thickness(0, 0, 0, 5);
    private static Thickness LeftMargin => new Thickness(0, 5, 0, 0);
    private static Thickness RightMargin => new Thickness(20, 5, 0, 0);
    private static Thickness LineMargin => new Thickness(20, 0, 20, 0);

    private static BoxView BuildHorizontalLine()
    {
        return new BoxView
        {
            Margin = LineMargin,
            HeightRequest = 2,
            HorizontalOptions = LayoutOptions.Fill,
            BackgroundColor = Colors.White
        };
    }

    private static Grid BuildGrid(List<(string left, IObservableValue<string> right)> rows)
    {
        var numRows = rows.Count;
        var grid = new Grid
        {
            HorizontalOptions = LayoutOptions.Fill,
            ColumnDefinitions = new ColumnDefinitionCollection(new ColumnDefinition[]
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Auto),
            }),
            RowDefinitions = new RowDefinitionCollection(
                Enumerable.Range(0, numRows).Select(_ => new RowDefinition()).ToArray()
            )
        };

        var c = 0;
        foreach (var row in rows)
        {
            grid.Add(new Label
            {
                Text = row.left,
                Margin = LeftMargin
            }, column: 0, row: c);

            var rightLabel = new Label
            {
                BindingContext = row.right,
                Margin = RightMargin
            };
            rightLabel.SetBinding(Label.TextProperty, "Value");
            grid.Add(rightLabel, column: 1, row: c);
            c++;
        }
        return grid;
    }

    private static List<(string, IObservableValue<string>)> ToObservable(List<(string left, string right)> pairs)
    {
        try
        {
            return pairs.Select(pair => (pair.left, (IObservableValue<string>)(new ObservableValue<string>(pair.right)))).ToList();
        }
        catch (Exception e)
        {
            ;
            throw new Exception();
        }
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        _vm.BuildTest();

        var spec = _vm.Specification;
        var problemSpec = spec.Problem;
        var problemDef = _vm.Provider.GetProblemDefinition(problemSpec);
        var problemInformation = _vm.Provider.GetProblemInformation(problemSpec);

        // Problem spec
        var problemRows = new List<(string left, string right)>();
        problemRows.Add(("Name:", problemSpec.Name));
        problemRows.Add(("Number of runs:", $"{spec.NumRuns}"));
        if (spec.ProblemSizes.Count == 1)
        {
            problemRows.Add(("Problem size:", $"{spec.ProblemSizes[0]}"));
        }
        if (spec.ProblemSizes.Count > 1)
        {
            var start = spec.ProblemSizes.First();
            var stop = spec.ProblemSizes.Last();
            var snd = spec.ProblemSizes[1];
            var step = snd - start;
            problemRows.Add(("Problem size start:", $"{start}"));
            problemRows.Add(("Problem size stop:", $"{stop}"));
            problemRows.Add(("Problem size step:", $"{step}"));
        }
        foreach (var kv in problemSpec.Parameters)
        {
            problemRows.Add(($"{kv.Key}:", $"{kv.Value}"));
        }
        var problemGrid = BuildGrid(ToObservable(problemRows));
        var problemLayout = new VerticalStackLayout
        {
            Margin = BoxMargin
        };
        problemLayout.Add(new Label
        {
            Text = "Problem",
            Margin = BoxTitleMargin,
            FontSize = TitleSize
        });
        problemLayout.Add(problemGrid);
        if (!string.IsNullOrWhiteSpace(problemInformation.Description))
        {
            problemLayout.Add(new Label
            {
                Text = problemInformation.Description,
                FontSize = 12,
                Margin = new Thickness(0, 10, 0, 0)
            });
        }
        Layout.Add(problemLayout);

        // Optimizers
        foreach (var optimizerSpec in spec.Optimizers)
        {
            try
            {

                Layout.Add(BuildHorizontalLine());

                var optimizerName = optimizerSpec.Name;
                var optLayout = new VerticalStackLayout
                {
                    Margin = BoxMargin
                };
                optLayout.Add(new Label
                {
                    Text = optimizerName,
                    Margin = BoxTitleMargin,
                    FontSize = TitleSize
                });
                var leftRows = new List<(string left, string right)>();
                leftRows.Add(("Algorithm:", optimizerSpec.Algorithm));
                if (optimizerSpec.Seed != null)
                {
                    leftRows.Add(("Seed:", $"{optimizerSpec.Seed.Value}"));
                }
                foreach (var kv in optimizerSpec.Parameters)
                {
                    leftRows.Add(($"{kv.Key}:", $"{kv.Value}"));
                }
                var leftStats = BuildGrid(ToObservable(leftRows));
                var leftCol = new VerticalStackLayout
                {
                    Margin = new Thickness(0, 5, 15, 0)
                };
                leftCol.Add(new Label
                {
                    Text = "Parameters",
                    FontSize = SubTitleSize
                });
                leftCol.Add(leftStats);

                var items = _vm.Test.Items.Where(item => item.OptimizerName == optimizerName);
                var rightRows = items.Select(item => ($"{item.Property.ToString()}:", item.Observable.ToStringObservable())).ToList();
                var rightStats = BuildGrid(rightRows);
                var rightCol = new VerticalStackLayout
                {
                    Margin = new Thickness(15, 5, 15, 0)
                };
                rightCol.Add(new Label
                {
                    Text = "Statistics",
                    FontSize = SubTitleSize
                });
                rightCol.Add(rightStats);

                var horizontal = new HorizontalStackLayout();
                horizontal.Add(leftCol);
                horizontal.Add(rightCol);

                optLayout.Add(horizontal);
                Layout.Add(optLayout);
            }
            catch (Exception e)
            {
                ;
            }

        }

        Layout.Add(BuildHorizontalLine());

        var propertyGroups = _vm.Test.Series.GroupBy(series => series.Property);
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

        try
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await _vm.RunTest(_cancellationTokenSource.Token);
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
                    GeometrySize = 0,
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
            var content = ResultExporter.Export(_vm.Test, _vm.Specification);
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