using System;
using System.Collections.Generic;
using System.Linq;

namespace TickspeedChallenge
{
    class Program
    {
        static List<Dimension> dimensions = new List<Dimension>();
        
        static void Main(string[] args)
        {
            for(int i=1; i <=8; i++)
                dimensions.Add(new Dimension(i));

            foreach(var d in dimensions) 
            {
                d.Step();
                Console.WriteLine(d.DimNumber + " " + d.StepsCount +  " " + d.CurrentCost);
            }

            foreach(var d in GetExisitingDimensions()) 
            {
                d.Step();
                Console.WriteLine(d.DimNumber + " " + d.StepsCount +  " " + d.CurrentCost);
            }            
        }

        static List<Dimension> GetExisitingDimensions()
        {
            if(dimensions.Single(x => x.DimNumber==8).StepsCount > 0)
                return dimensions;
            
            var existingDims = dimensions.Where(x => x.StepsCount > 0 || x.DimNumber == 1);
            var nextUnlocked=dimensions.Single(
                x => x.DimNumber == existingDims.Max(x => x.DimNumber)+1
            );
            existingDims.Append(nextUnlocked);

            return existingDims.ToList();
        }
    }
}
