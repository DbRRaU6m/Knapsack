using Knapsack.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Knapsack
{
    public static class PisingerImport
    {
        private static int ParseInt(string iString)
        {
            return Int32.Parse(new string(iString.Where(Char.IsDigit).ToArray()));
        }

        public static IEnumerable<KnapsackInstance<KnapsackItem>> Read(string FileName, out List<int> MaximumProfits)
        {
            List<KnapsackInstance<KnapsackItem>> Result = new List<KnapsackInstance<KnapsackItem>>();
            MaximumProfits = new List<int>();
            var Lines = File.ReadAllLines(FileName);
            KnapsackInstance<KnapsackItem> CurrentInstance = null;
            for (int i = 0; i < Lines.Count(); i++ )
            {
                if (null == CurrentInstance)            // beginning of instance
                {

                    var InstanceName = Lines[i++];
                    var N = Lines[i++];
                    var C = Lines[i++];
                    var Z = Lines[i++];
                    var Time = Lines[i];
                    CurrentInstance = new KnapsackInstance<KnapsackItem>();
                    CurrentInstance.Items = new List<KnapsackItem>();
                    CurrentInstance.Name = InstanceName;
                    CurrentInstance.Capacity = ParseInt(C);
                    MaximumProfits.Add(ParseInt(Z));
                }
                else
                {
                    if (Lines[i] == "-----")            // reached end of instance
                    {
                        i++;
                        Result.Add(CurrentInstance);
                        CurrentInstance = null;
                    }
                    else                                // read next item
                    {
                        var ItemLine = Lines[i];
                        var Parts = ItemLine.Split(',');
                        var iItem = new KnapsackItem
                        {
                            Profit = Int32.Parse(Parts[1]),
                            Weight = Int32.Parse(Parts[2])
                        };
                        CurrentInstance.Items.Add(iItem);
                    }
                }
            }
            return Result;
        }
    }
}
