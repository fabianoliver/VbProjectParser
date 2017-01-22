using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data._PROJECTINFORMATION;
using VbProjectParser.Data._PROJECTMODULES;

namespace VbProjectParser.Data
{
    /// <summary>
    /// Page 50
    /// </summary>
    public class ModuleStream : DataBase
    {
        //protected readonly DirStream dirStream;

        public Byte[] PerformanceCache { get; private set; }

        public Byte[] UncompressedSourceCode { get; private set; }

        protected readonly PROJECTINFORMATION ProjectInformation;

        public ModuleStream(PROJECTINFORMATION ProjectInformation, MODULE module, XlBinaryReader Data)
        {
            this.ProjectInformation = ProjectInformation;

            this.PerformanceCache = Data.ReadBytes(module.OffsetRecord.TextOffset);

            Byte[] rest = Data.GetUnreadData();
            var reader = new XlBinaryReader(ref rest);
            var container = new CompressedContainer(reader);
            var buffer = new DecompressedBuffer();
            container.Decompress(buffer);
            this.UncompressedSourceCode = buffer.GetData();
        }


        internal ModuleStream(PROJECTINFORMATION ProjectInformation, MODULE module, string SourceCode)
        {
            this.ProjectInformation = ProjectInformation;
            this.PerformanceCache = new Byte[] { };
            SetUncompressetSourceCode(SourceCode, ProjectInformation.CodePageRecord);
        }

        private void SetUncompressetSourceCode(string code, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(code);
            this.UncompressedSourceCode = bytes;
        }

        internal void SetUncompressetSourceCode(string code, PROJECTCODEPAGE Codepage)
        {
            SetUncompressetSourceCode(code, Codepage.GetEncoding());
        }

        internal void SetUncompressetSourceCode(string code, PROJECTINFORMATION ProjectInformation)
        {
            SetUncompressetSourceCode(code, ProjectInformation.CodePageRecord);
        }


        public string GetUncompressedSourceCodeAsString(Encoding encoding)
        {
            return encoding.GetString(this.UncompressedSourceCode);
        }

        public string GetUncompressedSourceCodeAsString(PROJECTCODEPAGE Codepage)
        {
            return GetUncompressedSourceCodeAsString(Codepage.GetEncoding());
        }

        public string GetUncompressedSourceCodeAsString(PROJECTINFORMATION ProjectInformation)
        {
            return GetUncompressedSourceCodeAsString(ProjectInformation.CodePageRecord);
        }

        public string GetUncompressedSourceCodeAsString()
        {
            return GetUncompressedSourceCodeAsString(this.ProjectInformation);
        }


    }
}
