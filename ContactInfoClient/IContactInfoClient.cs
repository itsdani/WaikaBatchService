using Waika.Model;

namespace Waika.ContactInfoClient
{
    public interface IContactInfoClient
    {
        ContactInfo FillOutContactInfo(ContactInfo requestInfo, string apiKey);
    }
}
