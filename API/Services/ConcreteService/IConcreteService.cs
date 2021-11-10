using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models.FieldConcreteTest;

namespace API.Services.ConcreteService
{
    public interface IConcreteService
    {
        IWebService<List<FieldConcreteTestDatumBaseJsonDataset>> concreteDatumJson { get; }
        IWebService<List<FieldConcreteTestFlattenJsonDataset>> concreteDatumFlatten { get; }

    }
}