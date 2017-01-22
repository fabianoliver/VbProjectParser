using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using VbProjectParser.Data;

namespace VbProjectParser.OpenXml
{
    /// <summary>
    /// Class that exposes members from VbaStorage in a more user-friendly way
    /// </summary>
    public class VbProject : IDisposable
    {
        private VbaStorage m_Storage;
        private IDisposable m_spreadsheetDisposable;

        private readonly Lazy<IEnumerable<VbModule>> m_Modules;
        public IEnumerable<VbModule> Modules
        {
            get { return m_Modules.Value; }
        }

        public string Name
        {
            get { return m_Storage.DirStream.InformationRecord.NameRecord.GetProjectNameAsString(); }
        }

        public VbProject(string PathToWorkbook)
            : this(LoadSpreadsheetDocumentFrom(PathToWorkbook), true)
        {
        }

        public VbProject(SpreadsheetDocument spreadsheetDocument)
            : this(spreadsheetDocument, false)
        {
        }

        private VbProject(SpreadsheetDocument spreadsheetDocument, bool DisposeSpreadsheetDocument)
            : this(spreadsheetDocument.WorkbookPart)
        {
            if (DisposeSpreadsheetDocument)
                this.m_spreadsheetDisposable = spreadsheetDocument;
        }

        public VbProject(WorkbookPart workbookPart)
            : this(GetVbaProjectPartFrom(workbookPart))
        {
        }

        public VbProject(VbaProjectPart vbaProjectPart)
            : this()
        {
            if (vbaProjectPart == null)
                throw new ArgumentNullException("vbaProjectPart");

            var stream = GetVbaStreamFrom(vbaProjectPart);
            this.m_Storage = new VbaStorage(stream);
        }

        public VbProject(VbaStorage vbaStorage)
            : this()
        {
            this.m_Storage = vbaStorage;
        }

        private VbProject()
        {
            this.m_Modules = new Lazy<IEnumerable<VbModule>>(CreateModuleInfos);
        }

        public void Dispose()
        {
            if (m_spreadsheetDisposable != null)
            {
                m_spreadsheetDisposable.Dispose();
                m_spreadsheetDisposable = null;
            }

            if (m_Storage != null)
            {
                m_Storage.Dispose();
                m_Storage = null;
            }
        }

        public VbaStorage AsVbaStorage()
        {
            return m_Storage;
        }

        private IEnumerable<VbModule> CreateModuleInfos()
        {
            foreach(var kvp in m_Storage.ModuleStreams)
            {
                string module_name = kvp.Key;
                ModuleStream module_stream = kvp.Value;
                string module_code = module_stream.GetUncompressedSourceCodeAsString();

                VbModule moduleInfo = new VbModule(module_name, module_code);
                yield return moduleInfo;
            }
        }

        private static SpreadsheetDocument LoadSpreadsheetDocumentFrom(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (!File.Exists(path))
                throw new FileNotFoundException(String.Format("File '{0}' not found", path), path);

            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(path, true);
            return spreadsheetDocument;
        }

        private static VbaProjectPart GetVbaProjectPartFrom(WorkbookPart workbookPart)
        {
            if (workbookPart == null)
                throw new ArgumentNullException("workbookPart");

            var allParts = workbookPart.GetPartsOfType<VbaProjectPart>();
            var vba = allParts.SingleOrDefault();
            return vba;
        }
     
        private static Stream GetVbaStreamFrom(VbaProjectPart vbaProjectPart)
        {
            if (vbaProjectPart == null)
                throw new ArgumentNullException("vbaProjectPart");

            var stream = vbaProjectPart.GetStream();
            return stream;
        }
    }
}
