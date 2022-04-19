using System.Collections.Generic;
using Application.FieldConcreteTest;
using Infrastructure.Core;

namespace Infrastructure.Imts.ConnectionService
{
    public interface IConnectionService
    {
        IWebService<List<FieldConcreteDatumDataset>> concreteDatumData { get; }
        IWebService<List<FieldConcreteDatumFlattenDataset>> concreteDatumFlattenData { get; }
        IWebService<List<FieldConcreteStrengthDataset>> concreteStrengthData { get; }
        IWebService<List<FieldConcreteMixNumberDataset>> concreteMixNumberData { get; }



    }
}