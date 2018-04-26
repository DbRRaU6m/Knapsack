namespace Knapsack.Solvers
{
    using Knapsack.Interface;
    using System.Collections.Generic;
    using System.Linq;

    public class BruteForce<T> : IKnapsackSolver<T> where T : IKnapsackItem
    {
        private void FillSubsets(List<List<T>> Subsets, IEnumerable<T> Items, IEnumerable<T> IntermediateSolution)
        {
            if (0 == Items.Count())                                                         // save leaves of search tree
            {
                Subsets.Add(IntermediateSolution.ToList());
            }
            else                                                                            // inner nodes of search tree
            {                                                                               // revaluate recursively
                var Head = Items.First();                                                   // first item
                var Tail = Items.Skip(1);                                                   // tail
                FillSubsets(Subsets, Tail, IntermediateSolution.Concat(new T[] { Head }));  // first item occurs in set
                FillSubsets(Subsets, Tail, IntermediateSolution);                           // first item does not occur in set
            }
        }

        public IEnumerable<T> Solve(KnapsackInstance<T> Instance)
        {
            var Subsets = new List<List<T>>();
            FillSubsets(Subsets, Instance.Items, new List<T>());
            var FeasibleSubsets = Subsets.Where( iList => iList.Sum(iItem => iItem.Weight) <= Instance.Capacity);
            return FeasibleSubsets.ArgMax(iList => iList.Sum(iItem => iItem.Profit));
        }
    }
}
