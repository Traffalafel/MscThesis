using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using CommunityToolkit.Mvvm.ComponentModel;

using MscThesis.Runner;
using MscThesis.UI.Views;

namespace MscThesis.UI
{
    public partial class TestViewModel
    {
        public string Text { get; set; } = "Hejsa!";

        public ISeries[] Series { get; set; } =
        {
            new LineSeries<double>
            {
                Values = new List<double> { 2, 1, 3, 5, 3, 4, 6, 6, 6 },
                Fill = null
            }
        };
    }

    public partial class MainPage : ContentPage
    {
        public TestViewModel VM = new TestViewModel();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = VM;

            //var runner = new TestRunner();
            //var result = runner.TestMIMIC();
            //var view = ResultsView.Create(result);
            //StackLayout.Add(view);


        }


        private void OnClicked(object sender, EventArgs e)
        {
        }
    }
}