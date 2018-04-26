namespace Knapsack.Solvers
{
    using Knapsack.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DpWeight<T> : IKnapsackSolver<T> where T : IKnapsackItem
    {
        private void InitializeStates(IList<T> Items, int[][] Profit, int N)
        {
            for (int i = 0; i < N; i++)                                                     // initialization of state space
            {
                Profit[i][0] = 0;
            }
            Profit[0][Items[0].Weight] = Items[0].Profit;
        }

        private void FillStatesParallel(IList<T> Items, int[][] Profit, int N, int W)       // parallel version of above method
        {
            for (int i = 1; i < N; i++)                                                     // evaluation of state space
            {
                Action<int> FillRow = j =>
                {
                    if (j - Items[i].Weight >= 0)                                           // item i can occur in solution
                    {
                        Profit[i][j] = Math.Max(Profit[i - 1][j - Items[i].Weight] + Items[i].Profit, Profit[i - 1][j]);
                    }
                    else                                                                    // item i cannot occur in solution
                    {
                        Profit[i][j] = Profit[i - 1][j];
                    }
                };
                Parallel.For(1, W, FillRow);
            }
        }

        private IEnumerable<T> Backtrack(IList<T> Items, int[][] Profit, int N, int OptimalWeight)
        {
            var Result = new List<T>();
            int j = OptimalWeight;                                                          // generation of an optimal solution via backtracking
            for (int i = N - 1; i > 0; i--)
            {
                if (j - Items[i].Weight >= 0)                                               // item i can occur in solution
                {
                    if (Profit[i - 1][j - Items[i].Weight] + Items[i].Profit > Profit[i - 1][j])
                    {
                        Result.Add(Items[i]);
                        j -= Items[i].Weight;
                    }
                }
            }
            if (Profit[0][j] > 0)
            {
                Result.Add(Items[0]);
            }
            return Result;
        }

        public IEnumerable<T> Solve(KnapsackInstance<T> Instance)                           // dp via weights
        {
            var N = Instance.Items.Count();
            var W = Instance.Capacity + 1;
            var MinusInfinity = -(Instance.Items.Sum(iItem => iItem.Profit) + 1);
            var Profit = MiniTools.Create(N, W, MinusInfinity);

            InitializeStates(Instance.Items, Profit, N);                                    // initialization of state space
            FillStatesParallel(Instance.Items, Profit, N, W);                               // evaluation of state space

            var LastRow = Profit[N - 1];                                                    // selection of weight of optimum
            var Pairs = LastRow.Select((p, w) => new Tuple<int, int>(w, p));
            var OptimalWeight = Pairs.ArgMax(t => t.Item2).Item1;

            return Backtrack(Instance.Items, Profit, N, OptimalWeight).Reverse();           // generate an optimal selection
        }
    }
}
