using System.Collections.Generic;
using System.Net.Http;
using API.Models.FieldConcreteTest;

namespace API.Services
{
    public class ConnectionService : IConnectionService
    {
        public HttpClient Client { get; }
        
        public ConnectionService(HttpClient client)
        {
            Client = client;

        }

        IWebService<List<FieldConcreteDatumDataset>> _concreteDatumData = null;

        IWebService<List<FieldConcreteDatumFlattenDataset>> _concreteDatumFlattenData = null;
        IWebService<List<FieldConcreteStrengthDataset>> _concreteStrengthData = null;
        IWebService<List<FieldConcreteMixNumberDataset>> _concreteMixNumberData = null;

        public IWebService<List<FieldConcreteDatumDataset>> concreteDatumData
        {
            get{
                if(_concreteDatumData == null)
                    _concreteDatumData = new WebService<List<FieldConcreteDatumDataset>>(Client);
                return _concreteDatumData;
            }
        }

        public IWebService<List<FieldConcreteDatumFlattenDataset>> concreteDatumFlattenData
        {
            get{
                if(_concreteDatumFlattenData == null)
                    _concreteDatumFlattenData = new WebService<List<FieldConcreteDatumFlattenDataset>>(Client);
                return _concreteDatumFlattenData;
            }
        }
        public IWebService<List<FieldConcreteStrengthDataset>> concreteStrengthData
        {
            get{
                if(_concreteStrengthData == null)
                    _concreteStrengthData = new WebService<List<FieldConcreteStrengthDataset>>(Client);
                return _concreteStrengthData;
            }
        }
        public IWebService<List<FieldConcreteMixNumberDataset>> concreteMixNumberData
        {
            get{
                if(_concreteMixNumberData == null)
                    _concreteMixNumberData = new WebService<List<FieldConcreteMixNumberDataset>>(Client);
                return _concreteMixNumberData;
            }
        }
        
       


    }
}