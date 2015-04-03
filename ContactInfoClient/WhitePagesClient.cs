using Newtonsoft.Json.Linq;
using RestSharp;
using Waika.Model;

namespace Waika.ContactInfoClient
{
    public class WhitePagesClient : IContactInfoClient
    {
        private static string PersonSearchURI = "https://proapi.whitepages.com/2.1/";
        public ContactInfo FillOutContactInfo(ContactInfo requestInfo, string apiKey)
        {
            
            var client = new RestClient(PersonSearchURI);
            var request = new RestRequest("person.json", Method.GET);
            request.AddParameter("api_key", apiKey);
            request.AddParameter("name", requestInfo.Name);
            request.AddParameter("address", requestInfo.Address);


            var response = client.Execute(request);
            JObject jObject = JObject.Parse(response.Content);
            dynamic data = jObject;
            



            return requestInfo;
        }
    }
}
