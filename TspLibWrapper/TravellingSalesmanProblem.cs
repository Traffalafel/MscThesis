using System;
using System.Collections.Generic;

namespace TspLibWrapper
{
    public class TravellingSalesmanProblem
    {
        public string Name { get; }
        public string Description { get; }

        public int Size { get; }

        public double? OptimalTourDistance { get; }

        public double TourDistance(IEnumerable<int> tour)
        {
            throw new NotImplementedException();
        }
    }
}
