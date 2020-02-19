using System;
using EncounterTracker.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var val = new ValidateHC();
            var check = val.ValidateNameField("mason");
            Assert.IsTrue(check);
        }
    }
}
