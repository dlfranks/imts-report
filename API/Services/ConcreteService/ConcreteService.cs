using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Threading.Tasks;
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
        public async Task<byte[]> createExcel(int projectId, int dataset)
        {

            DataTable tbl = new DataTable();

            var url = getUrl(projectId, dataset);
            url = url + "&format=excel";

            if (dataset == (int)FieldConcreteDatasetEnum.Full)
            {
                var data = await _connectService.concreteDatumFlattenData.OnGetData(url);
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(FieldConcreteDatumFlattenDataset));
                PropertyDescriptorCollection rowProps = TypeDescriptor.GetProperties(typeof(FieldConcreteDatumTestRowDataset));
                tbl = ConvertDataService.ConvertToTable<FieldConcreteDatumFlattenDataset>(data.Value, props, rowProps);
            }
            else if (dataset == (int)FieldConcreteDatasetEnum.Strength)
            {
                var data = await _connectService.concreteStrengthData.OnGetData(url);
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(FieldConcreteStrengthDataset));
                PropertyDescriptorCollection rowProps = TypeDescriptor.GetProperties(typeof(FieldConcreteStrengthRowDataset));
                tbl = ConvertDataService.ConvertToTable<FieldConcreteStrengthDataset>(data.Value, props, rowProps);
            }
            else if (dataset == (int)FieldConcreteDatasetEnum.MixNumber)
            {
                var data = await _connectService.concreteMixNumberData.OnGetData(url);
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(FieldConcreteMixNumberDataset));
                PropertyDescriptorCollection rowProps = TypeDescriptor.GetProperties(typeof(FieldConcreteMixNumberRowDataset));
                tbl = ConvertDataService.ConvertToTable<FieldConcreteMixNumberDataset>(data.Value, props, rowProps);
            }

            MemoryStream memoryStream = new MemoryStream();
            
            return await System.Threading.Tasks.Task.Run(() =>
            {
                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Concrete");
                    ws.Cells.LoadFromDataTable(tbl, true);
                    var dPos = new List<int>();
                    for (var i = 0; i < tbl.Columns.Count; i++)
                        if (tbl.Columns[i].DataType.Name.Equals("DateTime"))
                            dPos.Add(i);
                    foreach (var pos in dPos)
                    {
                        ws.Column(pos + 1).Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                    package.Save();
                    return memoryStream.ToArray();
                }
            });



        }

        public string getUrl(int projectId, int dataset)
        {
            return $"{baseUrl}?projectId={projectId}&dataset={dataset}";
        }
    }
}