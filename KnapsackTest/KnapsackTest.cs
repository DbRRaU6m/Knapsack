namespace KnapsackTest
{
    using Knapsack;
    using Knapsack.Interface;
    using Knapsack.Solvers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    [TestClass]
    public class KnapsackTest
    {
        private static string INSTANCES_DIR_NAME = "Instances";
        private static string SMALL_COEFFICIENTS_NAME = "SmallCoefficients";
        private static string LARGE_COEFFICIENTS_NAME = "LargeCoefficients";

        private static string FAILURE_FORMATSTRING = "Faild to solve instance {0}.";

        private static IEnumerable<string> GetFileNames(string iPath)
        {
            return Directory.EnumerateFiles(iPath).Where(iFileName => Path.GetFileName(iFileName).EndsWith(".csv"));
        }

        private void TestSingleInstanceDpProfit(KnapsackInstance<KnapsackItem> Instance, int Expected)
        {
            var GreedySolution = new Greedy<KnapsackItem>().Solve(Instance);
            var GreedyBound = GreedySolution.Sum(iItem => iItem.Profit) + Instance.Items.Max(iItem => iItem.Profit);
            var ActualSolution = new DpProfit<KnapsackItem>{ ProfitUpperBound = GreedyBound }.Solve(Instance);
            var Actual = ActualSolution.Sum(iItem => iItem.Profit);
            Assert.AreEqual(Expected, Actual, String.Format(FAILURE_FORMATSTRING, Instance.Name));
        }

        private void TestSingleInstanceDpWeight(KnapsackInstance<KnapsackItem> Instance, int Expected)
        {
            var ActualSolution = new DpWeight<KnapsackItem>().Solve(Instance);
            var Actual = ActualSolution.Sum(iItem => iItem.Profit);
            Assert.AreEqual(Expected, Actual, String.Format(FAILURE_FORMATSTRING, Instance.Name));
        }

        private void TestInstances(string DirName, int NumOfItems, Action<KnapsackInstance<KnapsackItem>,int> Test)
        {
            var FileNameInfix = String.Format("_{0}_", NumOfItems);
            var iPath = Path.Combine(INSTANCES_DIR_NAME, DirName);
            var iFileNames = GetFileNames(iPath).Where( iFileName => Path.GetFileName(iFileName).Contains(FileNameInfix));
            foreach (var iFileName in iFileNames)
            {
                List<int> MaximumProfits;
                var Instances = PisingerImport.Read(iFileName, out MaximumProfits).ToList();
                for (int i = 0; i < Instances.Count(); i++)
                {
                    var Instance = Instances[i];
                    var Expected = MaximumProfits[i];
                    Test(Instance, Expected);
                }
            }
        }

        #region small coefficients

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients50()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 50, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients100()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 100, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients200()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 200, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients500()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 500, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients1000()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 1000, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients10000()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 10000, TestSingleInstanceDpProfit);
        }

        #endregion

        #region large coefficients

        [TestMethod, TestCategory("LargeCoefficients"), TestCategory("DpProfit")]
        public void LargeCoefficients50()
        {
            TestInstances(LARGE_COEFFICIENTS_NAME, 50, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("LargeCoefficients"), TestCategory("DpProfit")]
        public void LargeCoefficients100()
        {
            TestInstances(LARGE_COEFFICIENTS_NAME, 100, TestSingleInstanceDpProfit);
        }

        /*
        [TestMethod, TestCategory("LargeCoefficients"), TestCategory("DpProfit")]
        public void LargeCoefficients200()
        {
            TestInstances(LARGE_COEFFICIENTS_NAME, 200, TestSingleInstanceDpProfit);
        }
        */

        #endregion

    }
}
