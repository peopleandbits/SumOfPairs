using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SumOfPairs
{
    public interface IPairSumSolver { bool HasPair(int[] numbers, int sum); int[] GetPair(int[] numbers, int sum); }
    
    public class NaivePairSumSolver : IPairSumSolver // O(n^2)
    {
        public bool HasPair(int[] numbers, int sum) => GetPair(numbers, sum) != null;

        public int[] GetPair(int[] numbers, int sum)
        {
            for (int i = 0; i < numbers.Length; i++)
                for (int j = i + 1; j < numbers.Length; j++)
                    if (numbers[i] + numbers[j] == sum)
                        return new[] { numbers[i], numbers[j] };

            return null;
        }
    }

    public class BestPairSumSolver : IPairSumSolver // O(n)
    {
        public bool HasPair(int[] numbers, int sum) => GetPair(numbers, sum) != null;

        public int[] GetPair(int[] numbers, int sum)
        {
            var complements = new HashSet<int>();

            foreach (var n in numbers)
            {
                if (complements.Contains(n)) return new[] { sum - n, n };
                complements.Add(sum - n);
            }

            return null;
        }
    }

    [TestClass]
    public class NaivePairSumSolverTest
    {
        [TestMethod]
        public void HasPair() => PairSumTestHelper.TestHasPair(new NaivePairSumSolver());

        [TestMethod]
        public void GetPair() => PairSumTestHelper.TestGetPair(new NaivePairSumSolver());
    }

    [TestClass]
    public class BestPairSumSolverTest
    {
        [TestMethod]
        public void HasPair() => PairSumTestHelper.TestHasPair(new BestPairSumSolver());

        [TestMethod]
        public void GetPair() => PairSumTestHelper.TestGetPair(new BestPairSumSolver());
    }

    public class PairSumTestHelper
    {
        static int[] _BigCase = Enumerable.Range(1, 9999).ToArray();
        static int[] _Case = new[] { 1, 2, 3, 4, 5 };

        public static void TestHasPair(IPairSumSolver solver)
        {
            solver.HasPair(_Case, 8).Should().BeTrue();
            solver.HasPair(_Case, 9).Should().BeTrue();
            solver.HasPair(_Case, 10).Should().BeFalse();

            solver.HasPair(_BigCase, 19997).Should().BeTrue();
            solver.HasPair(_BigCase, 19998).Should().BeFalse();
        }

        public static void TestGetPair(IPairSumSolver solver)
        {
            solver.GetPair(_Case, 8).Should().Equal(3, 5);
            solver.GetPair(_Case, 9).Should().Equal(4, 5);
            solver.GetPair(_Case, 10).Should().BeNull();

            solver.GetPair(_BigCase, 19997).Should().Equal(9998, 9999);
            solver.GetPair(_BigCase, 19998).Should().BeNull();
        }
    }
}