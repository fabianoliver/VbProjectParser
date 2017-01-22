using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VbProjectParser.Compression;
using System.Linq;

namespace VbProjectParserTests.Compression
{
    [TestClass]
    public class Case1 : BaseCase
    {
        public Case1()
        {
            this.TestData = new VbProjectParserTests.Data.Case1();
        }
    }
}
