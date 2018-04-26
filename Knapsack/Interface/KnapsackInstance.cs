namespace Knapsack.Interface
{
    using System.Collections.Generic;

    public class KnapsackInstance<T> where T :IKnapsackItem
    {
        public string Name { get; set; }
        public List<T> Items { get; set; }
        public int Capacity { get; set; }
    }
}
