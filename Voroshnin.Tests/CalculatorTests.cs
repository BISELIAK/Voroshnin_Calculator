using Microsoft.VisualStudio.TestTools.UnitTesting;
using Voroshnin.Lib;
using System;

namespace Voroshnin.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void TestAddition()
        {
            double result = Calculator.Execute(5, '+', 3);
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void TestSubtraction()
        {
            double result = Calculator.Execute(10, '-', 4);
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void TestMultiplication()
        {
            double result = Calculator.Execute(7, '*', 6);
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void TestDivision()
        {
            double result = Calculator.Execute(20, '/', 4);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void TestPower()
        {
            double result = Calculator.Execute(2, '^', 3);
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void TestDivisionByZero()
        {
            Calculator.Execute(10, '/', 0);
        }

        [TestMethod]
        public void TestRun()
        {
            double result = Calculator.Run("2+3");
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void TestRunPower()
        {
            double result = Calculator.Run("2^4");
            Assert.AreEqual(16, result);
        }
    }
}