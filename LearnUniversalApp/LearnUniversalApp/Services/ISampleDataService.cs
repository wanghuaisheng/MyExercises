using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using LearnUniversalApp.Models;

namespace LearnUniversalApp.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetSampleModelDataAsync();
    }
}
