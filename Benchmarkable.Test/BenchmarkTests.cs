﻿using Benchmarkable;
using NUnit.Framework;
using System;
using System.Threading;

namespace BenchmarkIt.Test
{
    [TestFixture ()]
	public class Test
	{
		[Test ()]
		public void TestSingle ()
		{
			var result = Benchmark.Just.This(() => {});

            Assert.IsTrue(result.Length == 1);
            Assert.AreEqual("Test 1", result[0].Label);
            Assert.Greater(result[0].OperationsPerSecond, 00);
            Assert.Greater(result[0].Variance, 0);
            Assert.Greater(result[0].BatchSize, 0);
            Assert.Greater(result[0].Error, 0);


            // Make sure this doesn't throw an exception
            result.Print();
		}

        [Test()]
        public void TestSingleWithException()
        {
            Assert.Catch<NullReferenceException>(() => Benchmark.Just.This(() => { throw new NullReferenceException(); }));
        }

        [Test()]
        public void ThrowsExceptionIfNoMethods()
        {
            var bench = new Benchmark();

            Assert.Catch<ArgumentOutOfRangeException>(() => bench.Run());
        }

        [Test()]
        public void TestSingleWithLabel()
        {
            var result = Benchmark.Just.This(() => { }, "My test");


            Assert.IsTrue(result.Length == 1);
            Assert.AreEqual("My test", result[0].Label);
            Assert.IsTrue(result[0].OperationsPerSecond > 0);
            Assert.IsTrue(result[0].Variance > 0);
        }

        [Test ()]
		public void TestDouble ()
		{
			var start = DateTime.Now;
            var result = Benchmark.This(() => { })
                .Against(() => { Thread.Sleep(250); });

            Assert.AreEqual("Test 1", result[0].Label);
            Assert.AreEqual("Test 2", result[1].Label);
            Assert.Greater(result[0].OperationsPerSecond, result[1].OperationsPerSecond);
		}

        [Test()]
        public void TestDoubleWithLabel()
        {
            var result = Benchmark.This(() => { }, "my test 1")
                .Against(() => { Thread.Sleep(250); }, "my test 2");

            Assert.AreEqual("my test 1", result[0].Label);
            Assert.AreEqual("my test 2", result[1].Label);
            Assert.IsTrue(result[0].OperationsPerSecond > result[1].OperationsPerSecond);
        }
        
        [Test()]
        public void TestMultipleWithLabels()
        {
            var result = Benchmark.These(new (Action, string)[]
            {
                (() => { }, "my test 1" ),
                (() => { }, "my test 2" ),
                (() => { }, "my test 3" ),
            });

            Assert.AreEqual(result.Length, 3);
            Assert.Greater(result[0].OperationsPerSecond, 0);
            Assert.Greater(result[1].OperationsPerSecond, 0);
            Assert.Greater(result[2].OperationsPerSecond, 0);
            Assert.AreEqual("my test 1", result[0].Label);
            Assert.AreEqual("my test 2", result[1].Label);
            Assert.AreEqual("my test 3", result[2].Label);
        }

        [Test()]
        public void TestExplicitUse()
        {
            var benchmark = new Benchmark();
            benchmark.Add(() => { }, "my test 1");

            var result = benchmark.Run();

            Assert.AreEqual(result.Length, 1);
            Assert.Greater(result[0].OperationsPerSecond, 0);
            Assert.AreEqual("my test 1", result[0].Label);
        }
    }
}

