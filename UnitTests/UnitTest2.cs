using System;
using Yahtzee;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod_Roll()
        {
            // arrange 
           
            List<int> dice = new List<int> {4,5,6};
            int numDie = 1;
            // act
            yahtzeeForm yahtzee = new yahtzeeForm();
            yahtzee.Roll(numDie, dice);
            int expected = 4;
            int actual = dice.Count;

            // assert
            Assert.AreEqual(expected, actual);
        }

    }
}
