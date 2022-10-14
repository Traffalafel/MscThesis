using MscThesis.UI.ViewModels;
using System.Collections.ObjectModel;

namespace MscThesis.UI.Views;

public partial class ExpressionParameterListSetup : ContentView
{
    public static readonly BindableProperty ParametersProperty = BindableProperty.Create(nameof(Parameters), typeof(ObservableCollection<ExpressionParameterVM>), typeof(ExpressionParameterListSetup), new ObservableCollection<ExpressionParameterVM>());

    public ObservableCollection<ExpressionParameterVM> Parameters
    {
        get => (ObservableCollection<ExpressionParameterVM>)GetValue(ParametersProperty);
        set => SetValue(ParametersProperty, value);
    }

    public ExpressionParameterListSetup()
	{
        InitializeComponent();
	}
}