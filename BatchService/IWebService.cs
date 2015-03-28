using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BatchService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IWebService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/ContactInfo/{id}")]
        Stream GetContactInfo(string id);

        [OperationContract]
        [WebInvoke(
            Method = WebRequestMethods.Http.Post,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/UploadCsv")]
        string UploadCsv(Stream stream);
    }
}
