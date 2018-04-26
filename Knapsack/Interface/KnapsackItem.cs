namespace Knapsack.Interface
{
    using System;

    public class KnapsackItem : IKnapsackItem
    {
        public int Profit { get; set; }
        public int Weight { get; set; }
        public string Name;
    }
}
