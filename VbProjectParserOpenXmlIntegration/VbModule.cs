using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParser.OpenXml
{
    public class VbModule
    {
        public string Name { get; private set; }
        public string Code { get; private set; }

        internal VbModule(string Name, string Code)
        {
            this.Name = Name;
            this.Code = Code;
        }
    }
}
