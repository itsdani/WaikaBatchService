using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Threading;
using Waika.ContactInfoClient;
using Waika.Model;

namespace Waika.BusinessLogic
{
    public class BatchProcessor
    {
        private readonly string _dir;
        public ConcurrentQueue<ContactBatch> BatchQueue { get; private set; }
        public IContactInfoClient ContactInfoClient { get; private set; }
        public BatchProcessor()
        {
            BatchQueue = new ConcurrentQueue<ContactBatch>();
            ContactInfoClient = new WhitePagesClient();
            _dir = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["storagepath"]);
        }
        public void Run()
        {
            while (true)
            {
                ContactBatch batch;
                if (BatchQueue.TryDequeue(out batch))
                {
                    foreach (var contact in batch.Contacts)
                    {
                        ContactInfoClient.FillOutContactInfo(contact, batch.ApiKey);
                    }
                    var path = Path.Combine(_dir, batch.Id + ".csv");
                    SaveFile(path, batch.ToCsv());
                }
                else
                {
                    Thread.Sleep(500);
                }
                
            }
        }

        private void SaveFile(string path, string fileContents)
        {
            CreateDirectoryIfNotExists(path);
            File.WriteAllText(path, fileContents);
        }

        private void CreateDirectoryIfNotExists(string filePath)
        {
            var directory = new FileInfo(filePath).Directory;
            if (directory == null) throw new Exception("Directory could not be determined for the filePath");

            Directory.CreateDirectory(directory.FullName);
        }
    }
}
