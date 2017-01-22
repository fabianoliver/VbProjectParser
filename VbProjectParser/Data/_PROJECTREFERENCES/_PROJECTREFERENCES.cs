using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;

namespace VbProjectParser.Data._PROJECTREFERENCES
{
    public class PROJECTREFERENCES : DataBase
    {
        public readonly REFERENCE[] ReferenceArray;

        public PROJECTREFERENCES(PROJECTINFORMATION ProjectInformation, XlBinaryReader Data)
        {
            var result = new List<REFERENCE>();

            // Read without progressing the pointer.
            // 0x000F indicates the beginning of a PROJECTMODULES Record, so we're done once we encounter that.
            while(Data.PeekUInt16() != 0x000F)
            {
                var reference = new REFERENCE(ProjectInformation, Data);
                result.Add(reference);
            }

            this.ReferenceArray = result.ToArray();
        }
    }
}
