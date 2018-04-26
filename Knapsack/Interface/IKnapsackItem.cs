
namespace Knapsack.Interface
{
    using System;

    public interface IKnapsackItem
    {
        int Profit { get; }
        int Weight { get; }
    }
}
