using System;
using Newtonsoft.Json.Linq;
using RestSharp;
using Waika.Model;

namespace Waika.ContactInfoClient
{
    public class WhitePagesClient : IContactInfoClient
    {
        private static string PersonSearchURI = "https://proapi.whitepages.com/2.1/";
        public RestClient Client { get; set; }

        public WhitePagesClient()
        {
            Client = new RestClient(PersonSearchURI);
        }


        public bool FillOutContactInfo(ContactInfo contactInfo, string apiKey)
        {
            bool isPersonFound = false;
            if (!String.IsNullOrWhiteSpace(contactInfo.Name))
            {
                isPersonFound = TryPersonUpdate(contactInfo, apiKey);
            }
            if (!isPersonFound && !String.IsNullOrWhiteSpace(contactInfo.PhoneNumber))
            {
                isPersonFound = TryPhoneUpdate(contactInfo, apiKey);
            }
            if (!isPersonFound && !String.IsNullOrWhiteSpace(contactInfo.Address))
            {
                isPersonFound = TryFullAddressUpdate(contactInfo, apiKey);
            }

            return isPersonFound;
        }

        private bool TryPersonUpdate(ContactInfo contactInfo, string apiKey)
        {
            bool isPersonFound = false;
            var request = new RestRequest("person.json", Method.GET);
            request.AddParameter("api_key", apiKey);
            request.AddParameter("name", contactInfo.Name);

            if (!String.IsNullOrWhiteSpace(contactInfo.Address))
            {
                request.AddParameter("address", contactInfo.Address);
            }
            if (!String.IsNullOrWhiteSpace(contactInfo.City))
            {
                request.AddParameter("city", contactInfo.City);
            }
            if (!String.IsNullOrWhiteSpace(contactInfo.State))
            {
                request.AddParameter("state_code", contactInfo.State);
            }
            if (!String.IsNullOrWhiteSpace(contactInfo.Zip))
            {
                request.AddParameter("postal_code", contactInfo.Zip);
            }

            var response = Client.Execute(request);
            JObject jObject = JObject.Parse(response.Content);
            dynamic data = jObject;

            if (data.results.Count == 1)
            {
                isPersonFound = true;
                FillAllDataFromPerson(contactInfo, data.results[0]);
            }
            return isPersonFound;
        }

        private void FillAllDataFromPerson(ContactInfo contactInfo, dynamic person)
        {
            if (String.IsNullOrWhiteSpace(contactInfo.Name))
            {
                try
                {
                    contactInfo.Name = person.best_name;
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
            if (String.IsNullOrWhiteSpace(contactInfo.Address))
            {
                try
                {
                    contactInfo.Address = person.locations[0].address;
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
            if (String.IsNullOrWhiteSpace(contactInfo.City))
            {
                try
                {
                    contactInfo.City = person.locations[0].city;
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
            if (String.IsNullOrWhiteSpace(contactInfo.State))
            {
                try
                {
                    contactInfo.State = person.locations[0].state_code;
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
            if (String.IsNullOrWhiteSpace(contactInfo.Zip))
            {
                try
                {
                    contactInfo.Zip = person.locations[0].postal_code;
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
            if (String.IsNullOrWhiteSpace(contactInfo.PhoneNumber))
            {
                try
                {
                    contactInfo.PhoneNumber = person.phones[0].phone_number;
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
        }

        private bool TryPhoneUpdate(ContactInfo contactInfo, string apiKey)
        {
            bool isPersonFound = false;
            var request = new RestRequest("phone.json", Method.GET);
            request.AddParameter("api_key", apiKey);
            request.AddParameter("phone_number", contactInfo.PhoneNumber);

            var response = Client.Execute(request);
            JObject jObject = JObject.Parse(response.Content);
            dynamic data = jObject;

            if (data.results.Count == 1 && data.results[0].belongs_to.Count>0)
            {
                isPersonFound = true;
                FillAllDataFromPerson(contactInfo, data.results[0].belongs_to[0]);
            }

            return isPersonFound;
        }

        private bool TryFullAddressUpdate(ContactInfo contactInfo, string apiKey)
        {
            bool isPersonFound = false;
            var request = new RestRequest("location.json", Method.GET);
            request.AddParameter("api_key", apiKey);
            request.AddParameter("address", contactInfo.Address);

            var response = Client.Execute(request);
            JObject jObject = JObject.Parse(response.Content);
            dynamic data = jObject;

            if (data.results.Count == 1 && data.results[0].legal_entities_at.Count > 0)
            {
                isPersonFound = true;
                FillAllDataFromPerson(contactInfo, data.results[0].legal_entities_at[0]);
            }

            return isPersonFound;
        }

    }
}
