using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using AntsCode.Util;
using Waika.ContactInfoClient;
using Waika.Model;

namespace Waika.BusinessLogic
{
    public class BatchHandler : IBatchHandler
    {
        private readonly string _dir;
        private readonly BatchProcessor _batchProcessor;
        private Thread processorThread;

        public BatchHandler()
        {
            _dir = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["storagepath"]);
            _batchProcessor = new BatchProcessor();
            processorThread = new Thread(_batchProcessor.Run);
            processorThread.Start();
        }

        //~BatchHandler()
        //{
        //    processorThread.Abort();
        //    processorThread.Join(500);
        //}

        public string AddBatch(Stream csvFileStream, string apiKey)
        {
            var contentString = GetContentString(csvFileStream);
            var batch = new ContactBatch(contentString, apiKey);
            _batchProcessor.BatchQueue.Enqueue(batch);
            
            return batch.Id.ToString();
        }

        public Stream GetContactInfo(string id)
        {
            var filename = id + ".csv";
            var path = Path.Combine(_dir, filename);
            if (!File.Exists(path))
                throw new FileNotFoundException();
            
            return File.OpenRead(path);
        }

        private static string GetContentString(Stream batchStream)
        {
            var parser = new MultipartParser(batchStream);

            if (!parser.Success)
            {
                throw new FileLoadException();
            }

            var contentString = Encoding.Default.GetString(parser.FileContents);
            return contentString;
        }
    }
}
