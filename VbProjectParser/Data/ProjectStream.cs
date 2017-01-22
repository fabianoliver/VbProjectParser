using AbnfFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VbProjectParser.Data._PROJECTINFORMATION;
using VbProjectParser.Data.ABNF;

namespace VbProjectParser.Data
{
    /* Example data:
     
        ID="{9BF9C398-EF64-4AC5-8C58-607535709BDA}"
        Document=ThisWorkbook/&H00000000
        Document=Sheet1/&H00000000
        Document=Sheet2/&H00000000
        Document=Sheet3/&H00000000
        Module=Module1
        Name="VBAProject"
        HelpContextID="0"
        VersionCompatible32="393222000"
        CMG="67659FB167715775577557755775"
        DPB="5351ABA596A696A696"
        GC="3F3DC7D95F294A2A4A2AB5"

        [Host Extender Info]
        &H00000001={3832D640-CF90-11CF-8E43-00A0C911005A};VBE;&H00000000

        [Workspace]
        ThisWorkbook=250, 250, 1046, 865, 
        Sheet1=0, 0, 0, 0, C
        Sheet2=0, 0, 0, 0, C
        Sheet3=0, 0, 0, 0, C
        Module1=275, 275, 1071, 890, Z

     * */

    /// <summary>
    /// Page 16 ff
    /// </summary>
    public class XlAbnfParser
    {
       
     


    }

    public class ProjectStream
    {
        public VBAPROJECTText ProjectText { get; set; }

        public ProjectStream(PROJECTCODEPAGE Codepage, Byte[] data)
        {
            string strData = Codepage.GetEncoding().GetString(data);
            SetValuesFromStr(strData);
        }

        public void SetValuesFromStr(string str)
        {
            var syntax = CreateAbnfSyntax();
            var entityBuilder = syntax.Entity<VBAPROJECTText>();

            bool canParse = entityBuilder.CanParse(str);

            if (!canParse)
            {
                string pattern = entityBuilder.GetRegexPattern();
                throw new InvalidOperationException(String.Format("Could not parse ABNF Syntax. Input was: {0}. Mapped regex pattern is: {1}", str, pattern));
            }

            var result = entityBuilder.FromAbnfSyntax(str);
           
#if DEBUG
            // When in debug mode, do a sanity check: convert the instantiated object back to ABNF syntax and check if it equals the initial string
            var _abnfSanityCheck = entityBuilder.ToAbnfSyntax(result);
            bool _abnfSanityCheckSuccess = String.Equals(str, _abnfSanityCheck);

            if (!_abnfSanityCheckSuccess)
                throw new InvalidOperationException(String.Format("ABNF data {0} did not re-map to itself; result was {1}", syntax, _abnfSanityCheck));
#endif

            this.ProjectText = result;
        }

        private ISyntax CreateAbnfSyntax()
        {
            var syntax = new Syntax();

            syntax.DefaultConverter.Add(CommonConverters.GuidConverter);
            syntax.DefaultConverter.Add(CommonConverters.Int32Converter);

            HostExtenderRef.Setup(syntax);
            HostExtenders.Setup(syntax);
            //LibidReference.Setup(syntax);
            ProjectId.Setup(syntax);
            ProjectDocModule.Setup(syntax);
            ProjectStdModule.Setup(syntax);
            ProjectClassModule.Setup(syntax);
            ProjectDesignerModule.Setup(syntax);
            ProjectPackage.Setup(syntax);
            ProjectProperties.Setup(syntax);
            ProjectReference.Setup(syntax);
            ProjectWindow.Setup(syntax);
            ProjectWindowState.Setup(syntax);
            ProjectWindowRecord.Setup(syntax);
            ProjectWorkspace.Setup(syntax);
            VBAPROJECTText.Setup(syntax);

            return syntax;
        }
    }
}
