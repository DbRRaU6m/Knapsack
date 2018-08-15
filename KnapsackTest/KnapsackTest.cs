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
            Instance = new KnapsackReducer<KnapsackItem>().Reduce(Instance);

            var GreedySolution = new Greedy<KnapsackItem>().Solve(Instance);
            var GreedyBound = GreedySolution.Sum(iItem => iItem.Profit) + Instance.Items.Max(iItem => iItem.Profit);
            var ActualSolution = new DpProfit<KnapsackItem>{ ProfitUpperBound = GreedyBound }.Solve(Instance);

            // System.GC.Collect();

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
        
        #region small coefficients dp profit

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients50DpProfit()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 50, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients100DpProfit()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 100, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients200DpProfit()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 200, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients500DpProfit()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 500, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients1000DpProfit()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 1000, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients2000DpProfit()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 2000, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients5000DpProfit()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 5000, TestSingleInstanceDpProfit);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpProfit")]
        public void SmallCoefficients10000DpProfit()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 10000, TestSingleInstanceDpProfit);
        }

        #endregion

        #region small coefficients dp weight

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpWeight")]
        public void SmallCoefficients50DpWeight()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 50, TestSingleInstanceDpWeight);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpWeight")]
        public void SmallCoefficients100DpWeight()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 100, TestSingleInstanceDpWeight);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpWeight")]
        public void SmallCoefficients200DpWeight()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 200, TestSingleInstanceDpWeight);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpWeight")]
        public void SmallCoefficients500DpWeight()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 500, TestSingleInstanceDpWeight);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpWeight")]
        public void SmallCoefficients1000DpWeight()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 1000, TestSingleInstanceDpWeight);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpWeight")]
        public void SmallCoefficients2000DpWeight()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 2000, TestSingleInstanceDpWeight);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpWeight")]
        public void SmallCoefficients5000DpWeight()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 5000, TestSingleInstanceDpWeight);
        }

        [TestMethod, TestCategory("SmallCoefficients"), TestCategory("DpWeight")]
        public void SmallCoefficients10000DpWeight()
        {
            TestInstances(SMALL_COEFFICIENTS_NAME, 10000, TestSingleInstanceDpWeight);
        }

        #endregion

    }
}
