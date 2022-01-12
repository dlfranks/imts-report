using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using API.Models.Core;
using API.Models.FieldConcreteTest;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace API.Services.ConcreteService
{
    public class ConcreteService
    {
        private readonly IConnectionService _connectService;

        private readonly IConfiguration _config;

        private readonly string baseUrl = "http://localhost:6777/ReportService/FieldConcreteJsonData";
        public ConcreteService(IConnectionService connectService, IConfiguration config)
        {
            _config = config;
            _connectService = connectService;

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
                    var datumResult = await _connectService.concreteDatumFlattenData.OnGetData(url);
                    PropertyDescriptorCollection flattenType = TypeDescriptor.GetProperties(typeof(FieldConcreteDatumFlattenDataset));
                    PropertyDescriptorCollection flattenRowType = TypeDescriptor.GetProperties(typeof(FieldConcreteDatumTestRowDataset));
                    
                    tbl = ConvertDataService.ConvertToTable<FieldConcreteDatumFlattenDataset>(datumResult.Value, flattenType, flattenRowType);
                    break;
                case 2:
                    var strengthResult = await _connectService.concreteStrengthData.OnGetData(url);
                    PropertyDescriptorCollection strengthType = TypeDescriptor.GetProperties(typeof(FieldConcreteStrengthDataset));
                    PropertyDescriptorCollection strengthRowType = TypeDescriptor.GetProperties(typeof(FieldConcreteStrengthRowDataset));
                    tbl = ConvertDataService.ConvertToTable<FieldConcreteStrengthDataset>(strengthResult.Value, strengthType, strengthRowType);
                    break;
                case 3:
                    var mixResult = await _connectService.concreteStrengthData.OnGetData(url);
                    PropertyDescriptorCollection mixType = TypeDescriptor.GetProperties(typeof(FieldConcreteMixNumberDataset));
                    PropertyDescriptorCollection mixRowType = TypeDescriptor.GetProperties(typeof(FieldConcreteMixNumberRowDataset));
                    tbl = ConvertDataService.ConvertToTable<FieldConcreteStrengthDataset>(mixResult.Value, mixType, mixRowType);
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
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(FieldConcreteDatumFlattenDataset));
                    PropertyDescriptorCollection rowProps = TypeDescriptor.GetProperties(typeof(FieldConcreteDatumTestRowDataset));
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
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(FieldConcreteStrengthDataset));
                    PropertyDescriptorCollection rowProps = TypeDescriptor.GetProperties(typeof(FieldConcreteStrengthRowDataset));
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
                    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(FieldConcreteMixNumberDataset));
                    PropertyDescriptorCollection rowProps = TypeDescriptor.GetProperties(typeof(FieldConcreteMixNumberRowDataset));
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
            return $"{baseUrl}?projectId={projectId}&dataset={dataset}";
        }
    }
}