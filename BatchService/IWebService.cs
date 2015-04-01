using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Waika.BatchService
{
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
            UriTemplate = "/UploadCsv?api_key={apiKey}")]
        string UploadCsv(Stream stream, string apiKey);
    }
}
