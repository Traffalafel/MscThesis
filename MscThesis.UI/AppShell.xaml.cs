using MscThesis.UI.Pages;

namespace MscThesis.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SetupPage), typeof(SetupPage));
            Routing.RegisterRoute(nameof(RunPage), typeof(RunPage));
            Routing.RegisterRoute(nameof(ResultPage), typeof(ResultPage));
        }
    }
}