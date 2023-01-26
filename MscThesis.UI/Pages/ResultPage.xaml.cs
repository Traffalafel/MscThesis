using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Extensions.Configuration;
using MscThesis.Core.Formats;
using MscThesis.Runner.Results;
using MscThesis.Runner;
using MscThesis.UI.ViewModels;
using SkiaSharp;
using MscThesis.Core;
using System.Collections.ObjectModel;
using LiveChartsCore;

namespace MscThesis.UI.Pages;

public partial class ResultPage : ContentPage
{
	private ResultVM _vm;
    private CancellationTokenSource _cancellationTokenSource;

    private string _resultsDir;

	public ResultPage(IConfiguration config)
	{
        var tspLibPath = config["TSPLibDirectoryPath"];
        _vm = new ResultVM(tspLibPath);

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

        var test = _vm.Test;
        var spec = _vm.Specification;
        var problemSpec = spec.Problem;
        var problemDef = _vm.Provider.GetProblemDefinition(problemSpec);
        var problemInformation = _vm.Provider.GetProblemInformation(problemSpec);

        if (_vm.ResultsFilePath != null)
        {
            // Get name 
            var name = Path.GetFileNameWithoutExtension(_vm.ResultsFilePath);
            var nameLabel = new Label
            {
                Text = $"Name: {name}",
                Margin = BoxMargin,
                FontSize = SubTitleSize
            };
            Layout.Add(nameLabel);
        }

        // Problem spec
        var problemRows = new List<(string left, string right)>();
        problemRows.Add(("Name:", problemSpec.Name));
        problemRows.Add(("Number of runs:", $"{spec.NumRuns}"));
        if (spec.ProblemSize != null)
        {
            problemRows.Add(("ProblemSize:", spec.ProblemSize.ToString()));
        }
        problemRows.Add(("Variable:", spec.Variable.ToString()));

        if (spec.VariableSteps != null)
        {
            problemRows.Add(("Variable start:", $"{spec.VariableSteps.Start}"));
            problemRows.Add(("Variable stop:", $"{spec.VariableSteps.Stop}"));
            problemRows.Add(("Variable step:", $"{spec.VariableSteps.Step}"));
        }

        if (problemSpec.Parameters != null)
        {
            foreach (var kv in problemSpec.Parameters)
            {
                problemRows.Add(($"{kv.Key}:", $"{kv.Value}"));
            }
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

        Layout.Add(BuildHorizontalLine());

        // Termination spec
        var terminationLayout = new VerticalStackLayout
        {
            Margin = BoxMargin
        };
        terminationLayout.Add(new Label
        {
            Text = "Termination criteria",
            Margin = BoxTitleMargin,
            FontSize = TitleSize
        });
        var c = 0;
        foreach (var terminationSpec in spec.Terminations)
        {
            if (c != 0)
            {
                terminationLayout.Add(new BoxView
                {
                    HeightRequest = 4,
                    HorizontalOptions = LayoutOptions.Fill,
                    BackgroundColor = Colors.Transparent,
                    Color = Colors.Transparent
                });
            }
            c++;

            var rows = new List<(string left, string right)>();
            rows.Add(("Name:", terminationSpec.Name));
            if (terminationSpec.Parameters != null)
            {
                foreach (var kv in terminationSpec.Parameters)
                {
                    rows.Add(($"{kv.Key}:", $"{kv.Value}"));
                }
            }
            var grid = BuildGrid(ToObservable(rows));
            terminationLayout.Add(grid);
        }
        Layout.Add(terminationLayout);

        var multipleSizes = spec.VariableSteps != null;

        // Optimizers
        foreach (var optimizerSpec in spec.Optimizers)
        {
            try
            {
                Layout.Add(BuildHorizontalLine());
                var horizontal = new HorizontalStackLayout();

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
                var paramsRows = new List<(string left, string right)>();
                paramsRows.Add(("Algorithm:", optimizerSpec.Algorithm));
                if (optimizerSpec.Parameters != null)
                {
                    foreach (var kv in optimizerSpec.Parameters)
                    {
                        paramsRows.Add(($"{kv.Key}:", $"{kv.Value}"));
                    }
                }
                var paramsGrid = BuildGrid(ToObservable(paramsRows));
                var paramsColumn = new VerticalStackLayout
                {
                    Margin = new Thickness(0, 5, 15, 0)
                };
                paramsColumn.Add(new Label
                {
                    Text = "Parameters",
                    FontSize = SubTitleSize
                });
                paramsColumn.Add(paramsGrid);
                horizontal.Add(paramsColumn);

                if (!multipleSizes)
                {
                    // Statistics
                    var items = _vm.Test.Items.Where(item => item.OptimizerName == optimizerName);
                    var statRows = items.Select(item => ($"{item.Property.ToString()}:", item.Observable.ToStringObservable())).ToList();
                    var statGrid = BuildGrid(statRows);
                    var statColumn = new VerticalStackLayout
                    {
                        Margin = new Thickness(15, 5, 15, 0)
                    };
                    statColumn.Add(new Label
                    {
                        Text = "Statistics",
                        FontSize = SubTitleSize
                    });
                    statColumn.Add(statGrid);
                    horizontal.Add(statColumn);

                    // Best solution view
                    var fittest = test.Fittest(optimizerName);
                    if (test.InstanceType == typeof(BitString))
                    {
                        var view = BuildBitStringView(fittest);
                        horizontal.Add(view);
                    }
                    if (test.InstanceType == typeof(Tour))
                    {
                        var view = BuildTourView(fittest);
                        horizontal.Add(view);
                    }
                }

                optLayout.Add(horizontal);
                Layout.Add(optLayout);
            }
            catch (Exception e)
            {
                ; // ignore
            }

        }

        // Series
        var seriesLayout = new VerticalStackLayout();
        Layout.Add(seriesLayout);

        var propertyGroups = _vm.Test.Series.GroupBy(series => series.Property);
        foreach (var propertyGroup in propertyGroups)
        {
            var propName = propertyGroup.Key.ToString();

            var series = BuildSeries(propertyGroup);

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
                WidthRequest = 700,
                Margin = new Thickness(5),
                SyncContext = _vm.Test.SeriesLock
            };

            var layout = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Start,
                Margin = BoxMargin
            };
            layout.Add(new Label
            {
                Text = $"{propName}:",
                FontSize = TitleSize,
                Margin = BoxTitleMargin,
                HorizontalOptions = LayoutOptions.Start
            });
            layout.Add(chart);

            seriesLayout.Add(BuildHorizontalLine());
            seriesLayout.Add(layout);
        }

        try
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _vm.RunTest(_cancellationTokenSource.Token);
        }
        catch (Exception e)
        {
            var msg = $"An exception was thrown with the message:\n{e.Message}";
            await DisplayAlert("Exception occured", msg, "Close");
        }
    }

    private View BuildBitStringView(IObservableValue<Individual<InstanceFormat>> observable)
    {
        var layout = BuildIndividualLayout();
        layout.WidthRequest = 600;

        double fitness = default;
        string value = string.Empty;
        if (observable != null && observable.Value != null)
        {
            fitness = observable.Value.Fitness.Value;
            value = observable.Value.Value.ToString();
        }
        var fitnessObservable = new ObservableValue<double>(fitness);
        observable.PropertyChanged += (s, e) =>
        {
            fitnessObservable.Value = observable.Value.Fitness.Value;
        };
        var fitnessHorizontal = new HorizontalStackLayout();
        fitnessHorizontal.Add(new Label
        {
            Text = "Fitness:",
            Margin = new Thickness(0, 0, 10, 5)
        });
        var fitnessLabel = new Label
        {
            BindingContext = fitnessObservable
        };
        fitnessLabel.SetBinding(Label.TextProperty, "Value");
        fitnessHorizontal.Add(fitnessLabel);
        layout.Add(fitnessHorizontal);

        layout.Add(new Label
        {
            Text = "Value:",
            Margin = new Thickness(0, 0, 0, 2)
        });

        var valueObservable = new ObservableValue<string>(value);
        observable.PropertyChanged += (s, e) =>
        {
            valueObservable.Value = observable.Value.Value.ToString();
        };
        var valueLabel = new Label
        {
            BindingContext = valueObservable,
            LineBreakMode = LineBreakMode.CharacterWrap
        };
        valueLabel.SetBinding(Label.TextProperty, "Value");
        layout.Add(valueLabel);

        return layout;
    }

    private View BuildTourView(IObservableValue<Individual<InstanceFormat>> observable)
    {
        var layout = BuildIndividualLayout();
        layout.WidthRequest = 600;

        double fitness = default;
        string value = string.Empty;
        if (observable != null && observable.Value != null)
        {
            fitness = observable.Value.Fitness.Value;
            value = observable.Value.Value.ToString();
        }
        var fitnessObservable = new ObservableValue<double>(fitness);
        observable.PropertyChanged += (s, e) =>
        {
            fitnessObservable.Value = observable.Value.Fitness.Value;
        };
        var fitnessHorizontal = new HorizontalStackLayout();
        fitnessHorizontal.Add(new Label
        {
            Text = "Fitness:",
            Margin = new Thickness(0, 0, 10, 5)
        });
        var fitnessLabel = new Label
        {
            BindingContext = fitnessObservable
        };
        fitnessLabel.SetBinding(Label.TextProperty, "Value");
        fitnessHorizontal.Add(fitnessLabel);
        layout.Add(fitnessHorizontal);

        layout.Add(new Label
        {
            Text = "Value:",
            Margin = new Thickness(0, 0, 0, 2)
        });

        var valueObservable = new ObservableValue<string>(value);
        observable.PropertyChanged += (s, e) =>
        {
            valueObservable.Value = observable.Value.Value.ToString();
        };
        var valueLabel = new Label
        {
            BindingContext = valueObservable,
            LineBreakMode = LineBreakMode.CharacterWrap
        };
        valueLabel.SetBinding(Label.TextProperty, "Value");
        layout.Add(valueLabel);

        return layout;
    }

    private VerticalStackLayout BuildIndividualLayout()
    {
        var layout = new VerticalStackLayout
        {
            Margin = new Thickness(15, 5, 15, 0)
        };
        var label = new Label
        {
            Text = "Best individual",
            FontSize = SubTitleSize,
            Margin = new Thickness(0, 0, 0, 5)
        };
        layout.Add(label);
        return layout;
    }

    private List<ISeries<(double,double)>> BuildSeries(IGrouping<Property, SeriesResult> groups)
    {
        return groups.Select<SeriesResult, ISeries<(double,double)>>(series =>
        {
            if (series.IsScatter)
            {
                // Points are not connected
                return new ScatterSeries<(double, double)>
                {
                    Values = series.Points,
                    Mapping = (value, point) =>
                    {
                        point.PrimaryValue = value.Item2;
                        point.SecondaryValue = value.Item1;
                    },
                    GeometrySize = 5,
                    Name = series.OptimizerName,
                };
            }
            else
            {
                // Points are connected
                return new LineSeries<(double, double)>
                {
                    Values = series.Points,
                    Mapping = (value, point) =>
                    {
                        point.PrimaryValue = value.Item2;
                        point.SecondaryValue = value.Item1;
                    },
                    Fill = null,
                    GeometrySize = 0,
                    Name = series.OptimizerName,
                    LineSmoothness = 0
                };
            }
        }).ToList();
    }

    private ObservableCollection<T> Combine<T>(IEnumerable<ObservableCollection<T>> collections)
    {
        var combined = new ObservableCollection<T>(collections.SelectMany(x => x));
        foreach (var col in collections)
        {
            col.CollectionChanged += (s, e) => { 
                foreach (var newItem in e.NewItems)
                {
                    combined.Add((T)newItem);
                }
            };
        }
        return combined;
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