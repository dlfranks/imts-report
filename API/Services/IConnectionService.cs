using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models.FieldConcreteTest;

namespace API.Services
{
    public interface IConnectionService
    {
        IWebService<List<FieldConcreteDatumDataset>> concreteDatumData { get; }
        IWebService<List<FieldConcreteDatumFlattenDataset>> concreteDatumFlattenData { get; }
        IWebService<List<FieldConcreteStrengthDataset>> concreteStrengthData { get; }
        IWebService<List<FieldConcreteMixNumberDataset>> concreteMixNumberData { get; }

        

    }
}