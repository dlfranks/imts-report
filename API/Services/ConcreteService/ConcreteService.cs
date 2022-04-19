using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Application.Core;
using Application.FieldConcreteTest;
using Infrastructure.Imts;
using Infrastructure.Imts.ConnectionService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace API.Services.ConcreteService
{
    public class ConcreteService
    {
        private readonly IConnectionService _connectService;
        private readonly string _baseUrl;
        public ConcreteService(IConnectionService connectService, IOptions<ImtsSettings> config)
        {
            
            _connectService = connectService;
            _baseUrl = config.Value.BaseUrl;

        }
        public async Task<string> getSamples()
        {

            int projectId = 6754;
            int length = Enum.GetNames(typeof(FieldConcreteDatasetEnum)).Length;
            var dataSet = new DataSet("dataset");

            string jsonString = "";

            for (int i = 0; i < length; i++)
            {
                int dataEnumDigit = i + 1;

                string url = getUrl(projectId, dataEnumDigit);



                DataTable tbl = await getWebService(dataEnumDigit, url);
                dataSet.Tables.Add(tbl);
            }
            jsonString = ConvertDataService.DataTableToJSON(dataSet);
            return jsonString;

        }
        public async Task<DataTable> getWebService(int dataEnum, string url)
        {

            DataTable tbl = new DataTable();


            switch (dataEnum)
            {
                case 1:
                    var datumResult = await _connectService.concreteDatumFlattenData.OnGetData(url, true);
                    if (datumResult.IsSuccess)
                    {
                        PropertyInfo[] flattenproperties = typeof(FieldConcreteDatumFlattenDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        PropertyInfo[] flattenRowsproperties = typeof(FieldConcreteDatumTestRowDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        // PropertyDescriptorCollection flattenType = TypeDescriptor.GetProperties(typeof(FieldConcreteDatumFlattenDataset));
                        // PropertyDescriptorCollection flattenRowType = TypeDescriptor.GetProperties(typeof(FieldConcreteDatumTestRowDataset));

                        tbl = ConvertDataService.ConvertToTable<FieldConcreteDatumFlattenDataset>(datumResult.Value, flattenproperties, flattenRowsproperties);
                    }
                    else
                    {
                        Console.WriteLine(datumResult.Error);
                    }


                    break;
                case 2:
                    var strengthResult = await _connectService.concreteStrengthData.OnGetData(url, true);
                    if (strengthResult.IsSuccess)
                    {
                        PropertyInfo[] strengthproperties = typeof(FieldConcreteStrengthDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        PropertyInfo[] strengthRowsproperties = typeof(FieldConcreteStrengthRowDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        tbl = ConvertDataService.ConvertToTable<FieldConcreteStrengthDataset>(strengthResult.Value, strengthproperties, strengthRowsproperties);
                    }
                    else
                    {
                        Console.WriteLine(strengthResult.Error);
                    }
                    break;
                case 3:

                    var mixResult = await _connectService.concreteMixNumberData.OnGetData(url, true);
                    if (mixResult.IsSuccess)
                    {
                        PropertyInfo[] mixproperties = typeof(FieldConcreteMixNumberDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        PropertyInfo[] mixRowsproperties = typeof(FieldConcreteMixNumberRowDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        tbl = ConvertDataService.ConvertToTable<FieldConcreteMixNumberDataset>(mixResult.Value, mixproperties, mixRowsproperties);
                    }
                    else
                    {
                        Console.WriteLine(mixResult.Error);
                    }

                    break;
            }

            return tbl;
        }
        public async Task<byte[]> createExcel(int projectId, int dataset)
        {

            DataTable tbl = new DataTable();

            var url = getUrl(projectId, dataset);
            url = url + "&format=excel";
            var result = new Result<byte[]>();

            if (dataset == (int)FieldConcreteDatasetEnum.Full)
            {

                var dataResult = await _connectService.concreteDatumFlattenData.OnGetData(url);
                if (dataResult.IsSuccess && dataResult.Value != null)
                {
                    PropertyInfo[] props = typeof(FieldConcreteDatumFlattenDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    PropertyInfo[] rowProps = typeof(FieldConcreteDatumTestRowDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    tbl = ConvertDataService.ConvertToTable<FieldConcreteDatumFlattenDataset>(dataResult.Value, props, rowProps);
                    result.IsSuccess = true;
                    result.Value = ConvertDataService.FromTableToExcel(tbl);

                }
                else
                {
                    result.Error = "Table Converting Error";
                }

            }
            else if (dataset == (int)FieldConcreteDatasetEnum.Strength)
            {
                var dataResult = await _connectService.concreteStrengthData.OnGetData(url);
                if (dataResult.IsSuccess && dataResult.Value != null)
                {
                    PropertyInfo[] props = typeof(FieldConcreteStrengthDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    PropertyInfo[] rowProps = typeof(FieldConcreteStrengthRowDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    tbl = ConvertDataService.ConvertToTable<FieldConcreteStrengthDataset>(dataResult.Value, props, rowProps);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Error = "Table Converting Error";
                }
            }
            else if (dataset == (int)FieldConcreteDatasetEnum.MixNumber)
            {
                var dataResult = await _connectService.concreteMixNumberData.OnGetData(url);
                if (dataResult.IsSuccess && dataResult.Value != null)
                {
                    PropertyInfo[] props = typeof(FieldConcreteMixNumberDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    PropertyInfo[] rowProps = typeof(FieldConcreteMixNumberRowDataset).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    tbl = ConvertDataService.ConvertToTable<FieldConcreteMixNumberDataset>(dataResult.Value, props, rowProps);
                    result.IsSuccess = true;
                }
                else
                {
                    result.Error = "Table Converting Error";
                }
            }

            MemoryStream memoryStream = new MemoryStream();

            return result.Value = ConvertDataService.FromTableToExcel(tbl);
        }

        public string getUrl(int projectId, int dataset)
        {
            return $"{_baseUrl}?projectId={projectId}&dataset={dataset}";
        }
    }
}