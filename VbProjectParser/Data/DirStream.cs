using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;
using VbProjectParser.Data._PROJECTMODULES;
using VbProjectParser.Data._PROJECTREFERENCES;

namespace VbProjectParser.Data
{
    public class DirStream
    {
        public readonly PROJECTINFORMATION InformationRecord;
        public readonly PROJECTREFERENCES ReferencesRecord;
        public readonly PROJECTMODULES ModulesRecord;

        public DirStream(XlBinaryReader Data)
        {
            this.InformationRecord = new PROJECTINFORMATION(Data);
            this.ReferencesRecord = new PROJECTREFERENCES(this.InformationRecord, Data);
            this.ModulesRecord = new PROJECTMODULES(this.InformationRecord, Data);
        }
    }
}
