using DocumentFormat.OpenXml.Packaging;
using OpenMcdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VbProjectParser.Compression;
using VbProjectParser.Data;
using VbProjectParser.OpenXml;

namespace VbProjectParserUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = true;
            while(run)
            {
                Console.WriteLine("Select one of the following actions:");
                Console.WriteLine("\t1 - Display test.vbaProject.bin contents");
                Console.WriteLine("\t2 - Read and display vbProject of a local Excel file on your disk");
                Console.WriteLine("\t3 - Replace the vbProject of a local Excel file with contents from a local .bin file");
                Console.WriteLine("\t4 or any other key - exit");
                Console.WriteLine();
                Console.Write("Your choice: ");

                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch(choice)
                {
                    case '1':
                        DisplayBinFile();
                        break;
                    case '2':
                        DisplayLocalExcelFile();
                        break;
                    case '3':
                        ReplaceVbaParts();
                        break;
                    default:
                        run = false;
                        break;
                }
            }

            Console.WriteLine("Done. The next keypress will close this window.");
            Console.ReadKey();
        }

        private static void DisplayBinFile()
        {
            ReadBinFile("test.vbaProject.bin");
        }

        private static void DisplayLocalExcelFile()
        {
            string path = null;
            while (String.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                Console.WriteLine("Enter full path to file: ");
                path = Console.ReadLine();

                if (!File.Exists(path))
                    Console.WriteLine("File '{0}' not found", path);
                else
                    Console.WriteLine();
            }

            ReadExcelFile(path);
        }

        private static void ReadBinFile(string path)
        {
            using (var VbaStorage = new VbaStorage(path))
            {
                PrintStorage(VbaStorage);
            }
        }

        private static void ReadExcelFile(string path)
        {
            using (var storage = new VbProject(path))
            {
                PrintStorage(storage);
            }
        }

        /// <summary>
        /// This function doesn't have anything to do specifically with this library, but is something that can be done
        /// simply with OpenXML
        /// </summary>
        private static void ReplaceVbaParts()
        {
            Console.WriteLine("Replace an existing vbProject of an Excel file with another vbProject");
            Console.WriteLine();

            Console.WriteLine("Enter path of workbook to open: ");
            string path = Console.ReadLine();

            if (!File.Exists(path))
                throw new FileNotFoundException(String.Format("File {0} does not exist", path));

            Console.WriteLine("Enter path of .bin file to open");
            string binPath = Console.ReadLine();

            if (!File.Exists(binPath))
                throw new FileNotFoundException(String.Format("File {0} does not exist", binPath));

            using (SpreadsheetDocument wb = SpreadsheetDocument.Open(path, true))
            {
                var wbPart = wb.WorkbookPart;

                using (var storage = new VbProject(wbPart))
                {
                    PrintStorage(storage);
                }

                Console.WriteLine();
                Console.WriteLine("---------------REPLACING VBA PART----------------");
                Console.WriteLine();

                // Replace parts
                var vba = wbPart
                    .GetPartsOfType<VbaProjectPart>()
                    .SingleOrDefault();

                if (vba != null)
                    wbPart.DeletePart(vba);

                VbaProjectPart newVbaPart = wbPart.AddNewPart<VbaProjectPart>();
                using (var stream = File.OpenRead(binPath))
                {
                    newVbaPart.FeedData(stream);
                }

                using (var storage = new VbProject(wbPart))
                {
                    PrintStorage(storage);
                }

                wbPart.Workbook.Save();
            }
        }

        private static void PrintStorage(VbProject VbProject)
        {
            PrintStorage(VbProject.AsVbaStorage());
        }

        private static void PrintStorage(VbaStorage VbaStorage)
        {
            if (VbaStorage == null)
                throw new ArgumentNullException("VbaStorage");

            Console.WriteLine("- - - VbaStorage - - -");
            Console.WriteLine("Project name: {0}", VbaStorage.DirStream.InformationRecord.NameRecord.GetProjectNameAsString());
            Console.WriteLine("Project docstring: {0}", VbaStorage.DirStream.InformationRecord.DocStringRecord.GetDocStringAsString());
            Console.WriteLine("Project constants: {0}", VbaStorage.DirStream.InformationRecord.ConstantsRecord.GetConstantsAsString());

            foreach (KeyValuePair<string, ModuleStream> ms in VbaStorage.ModuleStreams)
            {
                Console.WriteLine("\tModule stream: {0}", ms.Key);

                Console.WriteLine("Source code:");
                Console.WriteLine(ms.Value.GetUncompressedSourceCodeAsString());
            }
        }
    }
}
