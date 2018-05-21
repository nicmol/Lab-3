using System;
using Yahtzee;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod_Count()
        {
            // arrange 
            int value = 2;
            List<int> dice = new List<int> { 2, 2, 5, 2, 4 };

            // act
            yahtzeeForm yahtzee = new yahtzeeForm();
            int actual = yahtzee.Count(value, dice);
            int expected = 3;

            // assert
            Assert.AreEqual(expected, actual);
        }

    }
}
