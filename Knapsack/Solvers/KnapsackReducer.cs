namespace Knapsack.Solvers
{
    using Knapsack.Interface;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class KnapsackReducer<T> where T : IKnapsackItem
    {
        public KnapsackInstance<T> Reduce(KnapsackInstance<T> Instance)
        {
            var Result = new KnapsackInstance<T>
            {
                Name = Instance.Name,
                Capacity = Instance.Capacity,
                Items = new List<T>()
            };
            var FeasibleItems = Instance.Items.Where(iItem => iItem.Weight <= Instance.Capacity);   // keep only feasible items
            var ProfitableItems = FeasibleItems.Where( iItem => iItem.Profit > 0);                  // keep only profitable items
            var WeightGroups = ProfitableItems.GroupBy( iItem => iItem.Weight );                    // group items by weight

            foreach (var iGroup in WeightGroups)                                                    // keep only relevant items per group
            {
                var NumOfItems = (int)Math.Floor((double)Instance.Capacity / (double)iGroup.Key);
                var RelevantItems = iGroup.OrderByDescending(iItem => iItem.Profit).Take(NumOfItems);
                Result.Items.AddRange(RelevantItems);
            }

            var iMessage = String.Format("Removed {0} items from instance {1}.", Instance.Items.Count() - Result.Items.Count(), Instance.Name);
            Trace.WriteLine(iMessage);

            return Result;
        }
    }
}
