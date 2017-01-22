using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Data;

namespace VbProjectParser.OpenXml
{
    public static class VbaStorageExtensions
    {
        public static VbProject ToVbProject(this VbaStorage vbaStorage)
        {
            return new VbProject(vbaStorage);
        }
    }
}
