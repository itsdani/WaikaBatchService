using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waika.Model
{
    public class ContactInfo
    {
        public ContactInfo()
        {
        }
        public ContactInfo(string[] values, List<Columns> headerOrder)
        {
            for (int i = 0; i < headerOrder.Count; i++)
            {
                switch (headerOrder[i])
                {
                    case Columns.Name:
                        Name = values[i];
                        break;
                    case Columns.PhoneNumber:
                        PhoneNumber = values[i];
                        break;
                    case Columns.Address:
                        Address = values[i];
                        break;
                    case Columns.City:
                        City = values[i];
                        break;
                    case Columns.State:
                        State = values[i];
                        break;
                    case Columns.Zip:
                        Zip = values[i];
                        break;
                }
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set
            {
                _state = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }

        private string _zip;
        public string Zip
        {
            get { return _zip; }
            set
            {
                _zip = String.IsNullOrWhiteSpace(value) ? null : value.Trim();
            }
        }
    }
}
