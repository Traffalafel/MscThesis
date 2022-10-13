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

	public async void NavigateLoadResults(object sender, EventArgs e)
	{
        var fileResult = await FilePicker.Default.PickAsync();

		if (fileResult == null)
		{
			return;
		}

        await Shell.Current.GoToAsync(nameof(ResultPage), new Dictionary<string, object>
        {
            ["ResultsFilePath"] = fileResult.FullPath
        });
    }

}