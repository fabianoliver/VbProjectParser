using OpenMcdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data;
using VbProjectParser.Data._PROJECTINFORMATION;
using VbProjectParser.Data._PROJECTMODULES;

namespace VbProjectParser.Data
{
    public class VbaStorage : IDisposable
    {
        private IDisposable m_disposable;

        public _VBA_PROJECTStream _VBA_PROJECTStream { get; private set; }
        public ProjectStream ProjectStream { get; private set; }

        public readonly DirStream DirStream;
        
        /// <summary>
        /// Key: Name of the stream
        /// Value: Stream
        /// </summary>
        public readonly IReadOnlyDictionary<string, ModuleStream> ModuleStreams;/*
        {
            get
            {
                return new ReadOnlyDictionary<string, ModuleStream>(this._ModuleStreams);
            }
        }*/
        protected readonly IDictionary<string, ModuleStream> _ModuleStreams;

        public VbaStorage(CompoundFile VbaBinFile)
        {
            this.m_disposable = VbaBinFile;

            // _VBA_PROJECT stream
            var VBAStorage = VbaBinFile.RootStorage.GetStorage("VBA");
            this._VBA_PROJECTStream = ReadVbaProjectStream(VBAStorage);

            // DIR STREAM -------------------------
            CFStream thisWorkbookStream = VBAStorage.GetStream("dir");
            Byte[] compressedData = thisWorkbookStream.GetData();
            Byte[] uncompressed = XlCompressionAlgorithm.Decompress(compressedData);

            var uncompressedDataReader = new XlBinaryReader(ref uncompressed);
            this.DirStream = new DirStream(uncompressedDataReader);

            // MODULE STREAMS ----------------------------------------
            this._ModuleStreams = new Dictionary<string, ModuleStream>(DirStream.ModulesRecord.Modules.Length);
            this.ModuleStreams = new ReadOnlyDictionary<string, ModuleStream>(this._ModuleStreams);

            foreach (var module in DirStream.ModulesRecord.Modules)
            {
                var streamName = module.StreamNameRecord.GetStreamNameAsString();
                var stream = VBAStorage.GetStream(streamName).GetData();
                var localreader = new XlBinaryReader(ref stream);

                var moduleStream = new ModuleStream(DirStream.InformationRecord, module, localreader);

                this._ModuleStreams.Add(streamName, moduleStream);
            }

            // PROJECT stream
            CFStream ProjectStorage = VbaBinFile.RootStorage.GetStream("PROJECT");
            this.ProjectStream = ReadProjectStream(ProjectStorage, this.DirStream.InformationRecord.CodePageRecord);
        }

        private _VBA_PROJECTStream ReadVbaProjectStream(CFStorage VBAStorage)
        {
            CFStream stream = VBAStorage.GetStream("_VBA_PROJECT");
            Byte[] uncompressedData = stream.GetData();

            var dataReader = new XlBinaryReader(ref uncompressedData);
            var result = new _VBA_PROJECTStream(dataReader);
            return result;
        }

        private ProjectStream ReadProjectStream(CFStream PROJECTStorage, PROJECTCODEPAGE Codepage)
        {/*
            var y = new List<string>();
            PROJECTStorage.VisitEntries(x => y.Add(x.Name), false);*/
            Byte[] uncompressedData = PROJECTStorage.GetData();

            var stream = new ProjectStream(Codepage, uncompressedData);
            return stream;
        }

        internal VbaStorage(IDictionary<string, ModuleStream> ModuleStreams, DirStream DirStream)
        {
            this._ModuleStreams = ModuleStreams;
            this.ModuleStreams = new ReadOnlyDictionary<string, ModuleStream>(ModuleStreams);
        }


        public void Add(MODULE module, string SourceCode)
        {
            string moduleName = module.NameRecord.GetModuleNameAsString();
            string streamName = moduleName;

            if (this._ModuleStreams.ContainsKey(streamName))
                throw new ArgumentException(String.Format("Already contains a module stream named {0}", streamName), "moduleStream");

            var stream = new ModuleStream(this.DirStream.InformationRecord, module, SourceCode);
            throw new NotImplementedException();
            /*
            this._ModuleStreams.Add(streamName, moduleStream);
            ModuleStreamAdded(streamName, moduleStream);*/
        }

        // Called when an item has been added to this._ModuleStreams
        private void ModuleStreamAdded(string streamName, ModuleStream moduleStream)
        {
            if (this.DirStream == null)
                throw new NullReferenceException("DirStream was null");

            ++this.DirStream.ModulesRecord.Count;

        }

        public virtual void Dispose()
        {
            if (m_disposable != null)
                m_disposable.Dispose();
        }

        public VbaStorage(string PathToVbaBinFile)
            : this(new CompoundFile(PathToVbaBinFile))
        {
        }

        public VbaStorage(Stream VbaBinFileStream)
            : this(new CompoundFile(VbaBinFileStream))
        {
        }
    }
}
