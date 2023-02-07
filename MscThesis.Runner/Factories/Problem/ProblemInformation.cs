
namespace MscThesis.Runner.Factories.Problem
{
    public class ProblemInformation
    {
        public ProblemInformation()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        public ProblemInformation(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}
