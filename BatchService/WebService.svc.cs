﻿using System;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Web.Configuration;
using Waika.BusinessLogic;

namespace Waika.BatchService
{
    public class WebService : IWebService
    {
        private static readonly IBatchHandler BatchHandler = new BatchHandler();
        public Stream GetContactInfo(string id)
        {
            Stream result = null;
            try
            {
                result = BatchHandler.GetContactInfo(id);
            }
            catch (FileNotFoundException)
            {
                throw new WebFaultException<string>("The specified resource cannot be found", HttpStatusCode.NotFound);
            }

            if (WebOperationContext.Current == null) throw new WebFaultException(HttpStatusCode.InternalServerError);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/csv";
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Disposition", String.Format("attachment;filename={0}.csv", id));
            return result;
        }

        public string UploadCsv(Stream stream, string apiKey)
        {
            if (apiKey == null)
            {
                apiKey = WebConfigurationManager.AppSettings["defaultapikey"];
            }
            string result = null;
            try
            {
                result = BatchHandler.AddBatch(stream, apiKey);
            }
            catch (InvalidDataException)
            {
                throw new WebFaultException<string>("Error in the csv file", HttpStatusCode.BadRequest);
            }
            catch (FileLoadException)
            {
                throw new WebFaultException<string>("The posted file was not recognised.",
                    HttpStatusCode.BadRequest);
            }

            return result;
        }
    }
}
