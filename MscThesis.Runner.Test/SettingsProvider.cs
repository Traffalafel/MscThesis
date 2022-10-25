
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace MscThesis.Runner.Test
{
    public static class SettingsProvider
    {
        public static Settings Default => new Settings
        {
            TSPLibDirectoryPath = "C:\\Users\\traff\\Desktop\\TSPLIB95"
        };

        public static Settings Empty => new Settings
        {
            TSPLibDirectoryPath = string.Empty
        };
    }
}
