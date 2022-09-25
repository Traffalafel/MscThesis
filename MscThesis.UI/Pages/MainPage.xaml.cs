namespace MscThesis.UI.Pages;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	public void NavigateNewRun(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync(nameof(SetupPage));
	}

}