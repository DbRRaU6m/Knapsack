using Knapsack.Interface;
using System.Collections.Generic;

namespace Knapsack.Solvers
{
    public interface IKnapsackSolver<T> where T : IKnapsackItem
    {
        IEnumerable<T> Solve(KnapsackInstance<T> Instance);
    }
}
