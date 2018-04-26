namespace Knapsack.Solvers
{
    using Knapsack.Interface;
    using System.Collections.Generic;
    using System.Linq;

    public class Greedy<T> : IKnapsackSolver<T> where T : IKnapsackItem
    {
        public IEnumerable<T> Solve(KnapsackInstance<T> Instance)
        {
            int ResidualCapacity = Instance.Capacity;
            var OrderedItems = Instance.Items.OrderByDescending(iItem => (double)iItem.Profit / (double)iItem.Weight);
            return OrderedItems.Where( iItem => {
                                                    var Result = iItem.Weight <= ResidualCapacity;
                                                    ResidualCapacity -= Result ? iItem.Weight : 0;
                                                    return Result;
                                                }
            );
        }
    }
}
