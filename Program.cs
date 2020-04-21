using System;
using System.Collections.Generic;
using System.Linq;

namespace TickspeedChallenge
{
    class Program
    {
        static int maxDimensions = 8;
        static List<Dimension> dimensions = new List<Dimension>();

        static void Main(string[] args)
        {
            for (int i = 1; i <= maxDimensions; i++)
                dimensions.Add(new Dimension(i));

            do
            {
                var existingDims = GetExisitingDimensions().OrderBy(x => x.CurrentCost).ToList();

                bool found=false;
                foreach (var dim in existingDims)
                {
                    var otherDims = existingDims.ToList();
                    otherDims.Remove(dim);
                    
                    if (!dimensions.Exists(x => x.CurrentCost == dim.NextCost) && 
                        (
                            !IsUnlocking(dim) ||
                            (
                                IsUnlocking(dim) &&
                                !otherDims.Exists(x => x.CurrentCost == GetNextUnlocked(existingDims)?.CurrentCost)
                            ) 
                        )
                    ) 
                    {
                        found=true;
                        dim.Step(true);
                        Console.WriteLine($"Buy dimension {dim.DimNumber} !");
                        break;
                    }
                }

                if(!found)
                {
                    var dim = existingDims.OrderBy(x => x.CurrentCost).First();
                    dimensions.Where(x => x.CurrentCost == dim.NextCost).ToList().ForEach(x => x.Step(false));
                    dim.Step(true);
                    Console.WriteLine($"Buy dimension {dim.DimNumber} !");
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
