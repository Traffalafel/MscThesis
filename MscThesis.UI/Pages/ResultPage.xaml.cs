using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using MscThesis.Core.Formats;
using MscThesis.Core;
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

		Layout.Add(new Label
		{
			BindingContext = result.Fittest,
			Text = $"{result.Fittest.Value?.Fitness}"
		});

		var optimizerNames = result.GetOptimizerNames();
		foreach (var optimizerName in optimizerNames)
		{
            //var itemProps = result.GetItemProperties(optimizerName);
            //foreach (var itemProp in itemProps)
            //{
            //             var propName = itemProp.ToString();
            //             var observable = result.GetItemValue(optimizerName, itemProp);
            //             Layout.Add(new Label
            //             {
            //                 Text = $"{propName}: {observable.Value}"
            //             });
            //	observable.Subscribe((newVal) =>
            //	{
            //		// Reload label
            //	});
            //         }

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
        }

		await result.Execute();

		//foreach (var optimizerName in optimizerNames)
		//{
  //          Layout.Add(new Label
  //          {
  //              Text = $"{optimizerName}:"
		//	});
  //          var itemProps = result.GetItemProperties(optimizerName);
		//	foreach (var itemProp in itemProps)
		//	{
		//		var propName = itemProp.ToString();
		//		var observable = result.GetItemValue(optimizerName, itemProp);
		//		Layout.Add(new Label
		//		{
		//			Text = $"{propName}: {observable.Value}"
		//		});
		//	}
		//	var seriesProps = result.GetSeriesProperties(optimizerName);
		//	foreach (var seriesProp in seriesProps)
		//	{
  //              var propName = seriesProp.ToString();
  //              var values = result.GetSeriesValues(optimizerName, seriesProp);
  //              Layout.Add(new Label
  //              {
  //                  Text = $"{propName}:"
  //              });
		//		Layout.Add(new CartesianChart
		//		{
		//			Series = new List<ISeries> {
  //                      new LineSeries<double>
		//				{
		//					Values = values,
		//					Fill = null
		//				}
  //                  },
		//			HeightRequest = 400,
		//			WidthRequest = 600
		//		});
		//	}
		//}
    }

}