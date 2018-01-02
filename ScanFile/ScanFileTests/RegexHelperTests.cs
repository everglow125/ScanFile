using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScanFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanFile.Tests
{
    //[TestClass()]
    public class RegexHelperTests
    {
        [TestMethod()]
        public void MatchAfterUnitLengthTest()
        {
            string fileName = "2喷绘344X282CM.JPG";
            double[] length = new double[] { };
            var result = RegexHelper.MatchAfterUnitLength(fileName,ref length);
            Assert.AreEqual(3.44d, length[0]);
            Assert.AreEqual(2.82d, length[1]);
        }
    }
}