using System.Collections.Generic;
using System.Net.Http;
using Application.FieldConcreteTest;
using Infrastructure.Core;

namespace Infrastructure.Imts.ConnectionService
{
    public class ConnectionService : IConnectionService
    {
        public HttpClient _client;
        

        public ConnectionService(HttpClient client)
        {
            _client = client;
        }
        IWebService<List<FieldConcreteDatumDataset>> _concreteDatumData = null;
        IWebService<List<FieldConcreteDatumFlattenDataset>> _concreteDatumFlattenData = null;
        IWebService<List<FieldConcreteStrengthDataset>> _concreteStrengthData = null;
        IWebService<List<FieldConcreteMixNumberDataset>> _concreteMixNumberData = null;
        public IWebService<List<FieldConcreteDatumDataset>> concreteDatumData
        {
            get{
                if(_concreteDatumData == null)
                    _concreteDatumData = new WebService<List<FieldConcreteDatumDataset>>(_client);
                return _concreteDatumData;
            }
        }
        public IWebService<List<FieldConcreteDatumFlattenDataset>> concreteDatumFlattenData
        {
            get{
                if(_concreteDatumFlattenData == null)
                    _concreteDatumFlattenData = new WebService<List<FieldConcreteDatumFlattenDataset>>(_client);
                return _concreteDatumFlattenData;
            }
        }
        public IWebService<List<FieldConcreteStrengthDataset>> concreteStrengthData
        {
            get{
                if(_concreteStrengthData == null)
                    _concreteStrengthData = new WebService<List<FieldConcreteStrengthDataset>>(_client);
                return _concreteStrengthData;
            }
        }
        public IWebService<List<FieldConcreteMixNumberDataset>> concreteMixNumberData
        {
            get{
                if(_concreteMixNumberData == null)
                    _concreteMixNumberData = new WebService<List<FieldConcreteMixNumberDataset>>(_client);
                return _concreteMixNumberData;
            }
        }

        
        
    }

    
}