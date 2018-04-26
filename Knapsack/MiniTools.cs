namespace Knapsack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MiniTools
    {

        public static T ArgMax<T, R>(T t1, T t2, Func<T, R> f) where R : IComparable<R>
        {
            return f(t1).CompareTo(f(t2)) < 0 ? t2 : t1;
        }

        public static T ArgMax<T, R>(this IEnumerable<T> Sequence, Func<T, R> f) where R : IComparable<R>
        {
            return Sequence.Aggregate((t1, t2) => ArgMax<T, R>(t1, t2, f));
        }

        public static T[][] Create<T>(int NumOfRows, int NumOfColumns, T Value)
        {
            var Result = new T[NumOfRows][];
            for (int i = 0; i < NumOfRows; i++ )
            {
                Result[i] = new T[NumOfColumns];
                for (int j = 0; j < NumOfColumns; j++)
                {
                    Result[i][j] = Value;
                }
            }
            return Result;  
        }
    }
}
