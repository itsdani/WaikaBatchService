using System.IO;

namespace Waika.BusinessLogic
{
    public interface IBatchHandler
    {
        string AddBatch(Stream csvFileStream, string apiKey);
        Stream GetContactInfo(string id);
    }
}
