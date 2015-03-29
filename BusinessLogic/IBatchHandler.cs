using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IBatchHandler
    {
        string AddBatch(Stream batchStream, string apiKey);
        Stream GetResults(string id);
    }
}
