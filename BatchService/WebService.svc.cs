using System;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Configuration;
using AntsCode.Util;

namespace Waika.BatchService
{
    public class WebService : IWebService
    {
        private static readonly string Dir = Path.Combine(Directory.GetCurrentDirectory(), WebConfigurationManager.AppSettings["storagepath"]);
        public Stream GetContactInfo(string id)
        {
            var filename = id + ".csv";
            var path = Path.Combine(Dir, filename);
            if (!File.Exists(path))
                throw new WebFaultException<string>("The specified resource cannot be found", HttpStatusCode.NotFound);
            if (WebOperationContext.Current == null) throw new WebFaultException(HttpStatusCode.InternalServerError);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/csv";
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", "attachment;filename=" + filename);
            return File.OpenRead(path);
        }

        public string UploadCsv(Stream stream)
        {
            var guid = Guid.NewGuid();
            var parser = new MultipartParser(stream);
            var path = Path.Combine(Dir, guid + ".csv");
            string apiKey = WebConfigurationManager.AppSettings["defaultapikey"];
            if (WebOperationContext.Current.IncomingRequest.Headers["Api-Key"] != null)
            {
                apiKey = WebOperationContext.Current.IncomingRequest.Headers["Api-Key"];
            }
            if (parser.Success)
            {
                var contentString = Encoding.Default.GetString(parser.FileContents);
                SaveFile(path, contentString);
            }
            else
            {
                throw new WebFaultException<string>("The posted file was not recognised.", HttpStatusCode.UnsupportedMediaType);
            }
            return guid.ToString();
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
