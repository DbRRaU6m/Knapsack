namespace Knapsack.Solvers
{
    using Knapsack.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DpProfit<T> : IKnapsackSolver<T> where T : IKnapsackItem
    {
        public int ProfitUpperBound;                                                        // smaller upper bound may be known

        private void InitializeStates(IList<T> Items, int[][] Weight, int N)
        {
            for (int i = 0; i < N; i++)                                                     // initialization of state space
            {
                Weight[i][0] = 0;
            }
            Weight[0][Items[0].Profit] = Items[0].Weight;
        }

        /*
        private void FillStates(IList<T> Items, int[][] Weight, int N, int P)
        {
            for (int i = 1; i < N; i++)                                                     // evaluation of state space
            {
                for (int j = 1; j < P; j++)
                {
                    if (j - Items[i].Profit >= 0)                                           // item i can occur in solution
                    {
                        Weight[i][j] = Math.Min(Weight[i - 1][j - Items[i].Profit] + Items[i].Weight, Weight[i - 1][j]);
                    }
                    else                                                                    // item i cannot occur in solution
                    {
                        Weight[i][j] = Weight[i - 1][j];
                    }
                }
            }
        }
        */

        private void FillStatesParallel(IList<T> Items, int[][] Weight, int N, int P)       // parallel version of above method
        {
            for (int i = 1; i < N; i++)                                                     // evaluation of state space
            {
                Action<int> FillRow = j =>
                {
                    if (j - Items[i].Profit >= 0)                                           // item i can occur in solution
                    {
                        Weight[i][j] = Math.Min(Weight[i - 1][j - Items[i].Profit] + Items[i].Weight, Weight[i - 1][j]);
                    }
                    else                                                                    // item i cannot occur in solution
                    {
                        Weight[i][j] = Weight[i - 1][j];
                    }
                };
                Parallel.For(1, P, FillRow);
            }
        }

        private IEnumerable<T> Backtrack(IList<T> Items, int[][] Weight, int N, int OptimalProfit)
        {
            var Result = new List<T>();
            int j = OptimalProfit;                                                          // generation of an optimal solution via backtracking
            for (int i = N - 1; i > 0; i--)
            {
                if (j - Items[i].Profit >= 0)                                               // item i can occur in solution
                {
                    if (Weight[i - 1][j - Items[i].Profit] + Items[i].Weight < Weight[i - 1][j])
                    {
                        Result.Add(Items[i]);
                        j -= Items[i].Profit;
                    }
                }
            }
            if (Weight[0][j] > 0)
            {
                Result.Add(Items[0]);
            }
            return Result;
        }

        public IEnumerable<T> Solve(KnapsackInstance<T> Instance)                           // dp via profits
        {
            var N = Instance.Items.Count();
            var P = 0 == ProfitUpperBound ? Instance.Items.Sum(iItem => iItem.Profit) + 1 : ProfitUpperBound + 1;
            var PlusInfinity = Instance.Items.Sum(iItem => iItem.Weight) + 1;
            var Weight = MiniTools.Create(N, P, PlusInfinity);

            InitializeStates(Instance.Items, Weight, N);                                    // initialization of state space
            FillStatesParallel(Instance.Items, Weight, N, P);                               // evaluation of state space

            var LastRow = Weight[N - 1];                                                    // selection of maximum profit
            var Pairs = LastRow.Select((w, p) => new Tuple<int, int>(p, w));
            var OptimalProfit = Pairs.Where(t => t.Item2 <= Instance.Capacity).Max(t => t.Item1);

            return Backtrack(Instance.Items, Weight, N, OptimalProfit).Reverse();           // generate an optimal selection
        }
    }
}
