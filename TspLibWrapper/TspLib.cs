using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TspLibWrapper
{
    public class TspLib
    {
        private static string _tspDirectoryPath = Path.Join("TSPLIB95", "tsp");

        private Dictionary<string, TravellingSalesmanProblem> _problems;
        private List<string> _names;

        public TspLib()
        {
            if (!Directory.Exists(_tspDirectoryPath))
            {
                throw new Exception("Could not find TspLib directory");
            }

            _names = Directory.GetFiles(_tspDirectoryPath, "*.tsp").ToList();

            _problems = new Dictionary<string, TravellingSalesmanProblem>();
        }

        public List<string> Names => _names;

        public TravellingSalesmanProblem Load(string name)
        {
            if (_problems.ContainsKey(name))
            {
                return _problems[name];
            }

            if (!_names.Contains(name))
            {
                throw new NotImplementedException($"TSP instance does not exist: {name}");
            }

            var path = Path.Join(_tspDirectoryPath, $"{name}.tsp");
            var content = File.ReadAllText(path);

            var c = 0;
            var lines = content.Split('\n');

            throw new NotImplementedException();

        }

        private string TryFindOptimalTourContent(string name)
        {
            throw new NotImplementedException();
        }
    }
}
