using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;
using VbProjectParser.Data._PROJECTREFERENCES.ReferenceRecords;
using VbProjectParser.Data.Exceptions;

namespace VbProjectParser.Data._PROJECTREFERENCES
{
    public class REFERENCE : DataBase
    {
        public readonly REFERENCENAME NameRecord;
        public readonly REFERENCEORIGINAL ReferenceOriginalRecord;
        public readonly object ReferenceRecord;

        public REFERENCE(PROJECTINFORMATION ProjectInformation, XlBinaryReader Data)
        {
            this.NameRecord = new REFERENCENAME(ProjectInformation, Data);

            var peek = Data.PeekUInt16();
            
            if(peek == 0x002F)
            {
                this.ReferenceRecord = new REFERENCECONTROL(ProjectInformation,Data);
            }
            else if (peek == 0x0033)
            {
                this.ReferenceOriginalRecord = new REFERENCEORIGINAL(Data);

                // A reference original is followed immediately by a reference record.
                this.ReferenceRecord = new REFERENCECONTROL(ProjectInformation, Data);
            }
            else if (peek == 0x000D)
            {
                this.ReferenceRecord = new REFERENCEREGISTERED(Data);
            }
            else if (peek == 0x000E)
            {
                this.ReferenceRecord = new REFERENCEPROJECT(ProjectInformation, Data);
            }
            else
            {
                throw new WrongValueException("peek", peek, "0x002F, 0x0033, 0x000D or 0x000E");
            }

            Validate();
        }
    }
}
