using System;
using System.Collections.Generic;

namespace Waika.Model
{
    public class ContactBatch
    {
        private List<ContactInfo> _contacts;

        public List<ContactInfo> Contacts
        {
            get
            {
                if (_contacts == null)
                {
                    _contacts = new List<ContactInfo>();
                }
                return _contacts;
            }
            set { _contacts = value; }
        }
        public string ApiKey { get; set; }
        public Guid Id { get; private set; }

        public ContactBatch(Guid id, string apiKey)
        {
            Id = id;
            ApiKey = apiKey;
        }
    }
}
