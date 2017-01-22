using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Data.ABNF;
using VbProjectParser.Data.ABNF.Enums;

namespace VbProjectParserTests.ABNF
{
    [TestClass]
    public class ProjectReferenceTests
    {

        private const string _ValidStr = @"*\AC:\some_path\to\afile.abc";
        private const string _Invalid1 = @"AC:\somne_path\toafile.abc";
        private const string _Invalid2 = @"*\ZC:\some_Path\to\afile.abc";

        [TestMethod]
        public void ABNF_ProjectReference_Parse_Valid_Test()
        {
            var x = new TestAbnfObject();
            x.TestStr = "HELLOONE";
            x.TestSubObject = new TestSubObject();
            x.TestSubObject.SubPath = "HELLOTWO";
            x.TestSubObject.SubPath2 = "HELLOTHREE";
            var y = x.GetRegexPattern();

            var stringRep = x.ToString();
            var newObj = new TestAbnfObject();
            newObj.InitializeFrom(stringRep);

            string text = _ValidStr;
            var projectReference = new ProjectReference();

            projectReference.InitializeFrom(text);

            Assert.IsNotNull(projectReference);
            Assert.AreEqual(projectReference.ProjectKind, ProjectKind.Standalone_Windows);
            Assert.AreEqual(projectReference.ProjectPath, @"C:\some_path\to\afile.abc");
        }

        [TestMethod]
        public void ABNF_ProjectReference_ToStr_Test()
        {
            string text = _ValidStr;
            var projectReference = new ProjectReference();
            projectReference.InitializeFrom(text);
            string str = projectReference.ToString();

            Assert.IsNotNull(str);
            Assert.AreEqual(str, text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ABNF_ProjectReference_Parse_Invalid1_Test()
        {
            string text = _Invalid1;
            var projectReference = new ProjectReference();
            projectReference.InitializeFrom(text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ABNF_ProjectReference_Parse2_Invalid2_Test()
        {
            string text = _Invalid2;
            var projectReference = new ProjectReference();
            projectReference.InitializeFrom(text);
        }
    }
}
