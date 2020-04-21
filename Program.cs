using System;
using System.Collections.Generic;
using System.Linq;

namespace TickspeedChallenge
{
    class Program
    {
        static int maxDimensions = 8;
        static int maxCostInterval = 10;
        static List<Dimension> dimensions = new List<Dimension>();

        static void Main(string[] args)
        {
            for (int i = 1; i <= maxDimensions; i++)
                dimensions.Add(new Dimension(i));

            do
            {
                var existingDims = GetExisitingDimensions().OrderBy(x => x.CurrentCost).ToList();
                var cheapest = existingDims.First();

                bool found=false;
                foreach (var dim in existingDims)
                {
                    var otherDims = dimensions.ToList();
                    otherDims.Remove(dim);
                    
                    if (!otherDims.Exists(x => x.CurrentCost == dim.CurrentCost) &&
                        dim.CurrentCost - cheapest.CurrentCost < maxCostInterval)
                    {
                        found=true;
                        Console.WriteLine($"Buy dimension {dim.DimNumber} ! ({dim.CurrentCost})");
                        dim.Step(true);
                        break;
                    }
                }

                if(!found)
                {
                    var dim = existingDims
                        .OrderBy(x => x.CurrentCost)
                        .ThenByDescending(x => x.DimNumber)
                        .First();
                    var conflicts = dimensions
                        .Where(x => 
                            x.CurrentCost == dim.CurrentCost && 
                            x.DimNumber != dim.DimNumber)
                        .ToList();

                    conflicts.ForEach(x => x.Step(false));
                    Console.WriteLine($"Buy dimension {dim.DimNumber} ! ({dim.CurrentCost}) [{string.Join(",", conflicts.Select(x => x.DimNumber))}]");
                    dim.Step(true);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        static bool IsUnlocking(Dimension dim) {
            var maxDim = GetExisitingDimensions().Max(x => x.DimNumber);
            return  maxDim == dim.DimNumber && maxDim < maxDimensions;
        }

        static Dimension GetNextUnlocked(IEnumerable<Dimension> dims)
        {
            if (dimensions.Single(x => x.DimNumber == maxDimensions).StepsCount > 0)
                return null;

            var nextUnlocked = dimensions.SingleOrDefault(
                x => x.DimNumber == dims.Max(x => x.DimNumber) + 1
            );

            return nextUnlocked;
        }

        static List<Dimension> GetExisitingDimensions()
        {
            var existingDims = dimensions.Where(x => 
                x.DimNumber == 1 ||
                dimensions.Exists(y => 
                    y.DimNumber == x.DimNumber - 1 && 
                    y.VisibleStepsCount > 0
                )
            ).ToList();
            return existingDims.ToList();
        }
    }
}
