using Waika.Model;

namespace Waika.ContactInfoClient
{
    public interface IContactInfoClient
    {
        bool FillOutContactInfo(ContactInfo contactInfo, string apiKey);
    }
}
