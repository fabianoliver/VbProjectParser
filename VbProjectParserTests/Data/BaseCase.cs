using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VbProjectParserTests.Data
{
    public abstract class BaseCase : ICompressionData
    {
        public Byte[] UncompressedData { get; protected set; }
        public Byte[] CompressedData { get; protected set; }
    }
}
