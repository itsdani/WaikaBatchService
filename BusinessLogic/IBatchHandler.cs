using System.IO;

namespace BusinessLogic
{
    public interface IBatchHandler
    {
        string AddBatch(Stream batchStream, string apiKey);
        Stream GetResults(string id);
    }
}
