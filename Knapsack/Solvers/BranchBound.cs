namespace Knapsack.Solvers
{
    using Knapsack.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BranchBound<T> : IKnapsackSolver<T> where T : IKnapsackItem
    {
        private class Selection                     // private type for representation of values of
        {                                           // feasible solutions for branch and bound
            public int TotalProfit { get; set; }
            public int TotalWeight { get; set; }
        }

        private List<Selection> GenerateSuccessors(List<Selection> Solutions, IKnapsackItem iItem, int Capacity)
        {
            var Result = Solutions.Select(iSol => new Selection { TotalProfit = iSol.TotalProfit + iItem.Profit,
                                                                  TotalWeight = iSol.TotalWeight + iItem.Weight })
                                  .Where(iSol => iSol.TotalWeight <= Capacity)
                                  .ToList();
            return Result;
        }

        private void FillSolutions(KnapsackInstance<T> Instance, List<Selection> Solutions)
        {
            foreach (var iItem in Instance.Items)
            {
                var Successors = GenerateSuccessors(Solutions, iItem, Instance.Capacity);

                // todo merge and dominate

            }
        }

        public IEnumerable<T> Solve(KnapsackInstance<T> Instance)
        {
            var InitialSolution = new Selection { TotalProfit = 0, TotalWeight = 0 };   // initial solution is empty
            var Solutions = new List<Selection>(new Selection[] { InitialSolution });

            // todo generate solution

            throw new NotImplementedException();
        }
    }
}
