using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;

namespace VbProjectParser.Data._PROJECTINFORMATION
{
    /// <summary>
    /// Page 31
    /// </summary>
    public class PROJECTINFORMATION : DataBase
    {
        public readonly PROJECTSYSKIND SysKindRecord;
        public readonly PROJECTCOMPATVERSION ProjectCompatVersion;
        public readonly PROJECTLCID LcidRecord;
        public readonly PROJECTLCIDINVOKE LcidInvokeRecord;
        public readonly PROJECTCODEPAGE CodePageRecord;
        public readonly PROJECTNAME NameRecord;
        public readonly PROJECTDOCSTRING DocStringRecord;
        public readonly PROJECTHELPFILEPATH HelpFilePathRecord;
        public readonly PROJECTHELPCONTEXT HelpContextRecord;
        public readonly PROJECTLIBFLAGS LibFlagsRecord;
        public readonly PROJECTVERSION VersionRecord;
        public readonly PROJECTCONSTANTS ConstantsRecord;

        public PROJECTINFORMATION(XlBinaryReader Data)
        {
            this.SysKindRecord = new PROJECTSYSKIND(Data);

            // Excel 2019 includes a new PROJECTCOMPATVERSION Record (section 2.3.4.2.1.2).
            // Check if the project has a PROJECTCOMPATVERSION record.
            int nextRecordType = Data.PeekUInt16();
            if (nextRecordType == PROJECTCOMPATVERSION.RecordId)
            {
                this.ProjectCompatVersion = new PROJECTCOMPATVERSION(Data);
            }

            this.LcidRecord = new PROJECTLCID(Data);
            this.LcidInvokeRecord = new PROJECTLCIDINVOKE(Data);
            this.CodePageRecord = new PROJECTCODEPAGE(Data);
            this.NameRecord = new PROJECTNAME(this,Data);
            this.DocStringRecord = new PROJECTDOCSTRING(this, Data);
            this.HelpFilePathRecord = new PROJECTHELPFILEPATH(this, Data);
            this.HelpContextRecord = new PROJECTHELPCONTEXT(Data);
            this.LibFlagsRecord = new PROJECTLIBFLAGS(Data);
            this.VersionRecord = new PROJECTVERSION(Data);
            this.ConstantsRecord = new PROJECTCONSTANTS(this, Data);

            Validate();
        }
    }
}
