using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AntsCode.Util;

namespace Waika.BusinessLogic
{
    public class BatchHandler : IBatchHandler
    {
        private static string Dir;

        public BatchHandler()
        {
            Dir = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["storagepath"]);
        }

        public string AddBatch(Stream batchStream, string apiKey)
        {
            var guid = Guid.NewGuid();
            var parser = new MultipartParser(batchStream);
            var path = Path.Combine(Dir, guid + ".csv");
            
            if (parser.Success)
            {
                var contentString = Encoding.Default.GetString(parser.FileContents);
                SaveFile(path, contentString);
            }
            else
            {
                throw new InvalidDataException();
            }
            return guid.ToString();
        }

        public Stream GetContactInfo(string id)
        {
            var filename = id + ".csv";
            var path = Path.Combine(Dir, filename);
            if (!File.Exists(path))
                throw new FileNotFoundException();
            
            return File.OpenRead(path);
        }

        private static void SaveFile(string path, string fileContents)
        {
            CreateDirectoryIfNotExists(path);
            File.WriteAllText(path, fileContents);
        }

        private static void CreateDirectoryIfNotExists(string filePath)
        {
            var directory = new FileInfo(filePath).Directory;
            if (directory == null) throw new Exception("Directory could not be determined for the filePath");

            Directory.CreateDirectory(directory.FullName);
        }
    }
}
