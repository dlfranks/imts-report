using System.Collections.Generic;
using System.Net.Http;
using API.Models.FieldConcreteTest;

namespace API.Services.ConcreteService
{
    public class ConcreteService : IConcreteService
    {
        public HttpClient Client { get; }

        
        public ConcreteService(HttpClient client)
        {
            Client = client;

        }

        IWebService<List<FieldConcreteTestDatumBaseJsonDataset>> _concreteDatumJson = null;

        IWebService<List<FieldConcreteTestFlattenJsonDataset>> _concreteDatumFlatten = null;

        public IWebService<List<FieldConcreteTestDatumBaseJsonDataset>> concreteDatumJson
        {
            get{
                if(_concreteDatumJson == null)
                    _concreteDatumJson = new WebService<List<FieldConcreteTestDatumBaseJsonDataset>>(Client);
                return _concreteDatumJson;
            }
        }

        public IWebService<List<FieldConcreteTestFlattenJsonDataset>> concreteDatumFlatten
        {
            get{
                if(_concreteDatumFlatten == null)
                    _concreteDatumFlatten = new WebService<List<FieldConcreteTestFlattenJsonDataset>>(Client);
                return _concreteDatumFlatten;
            }
        }
        




    }
}