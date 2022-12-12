using System;
using System.Threading.Tasks;
using Application.FieldConcreteTest;
using API.Services;
using API.Services.ConcreteService;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Imts.ConnectionService;
using Application.Interfaces;

namespace API.Controllers
{

    public class FieldConcreteTestController : BaseApiController
    {
        private readonly IConnectionService _connectionService;
        private readonly ConcreteService _concreteService;
        public FieldConcreteTestController(
            IConnectionService connectionService,
            ConcreteService concreteService,
            IUserAccessor userAccessor,
            UserService userService
            ) : base(userAccessor, userService)
        {
            _concreteService = concreteService;
            _connectionService = connectionService;
        }

        [Route("samples")]
        [HttpGet]
        public async Task<IActionResult> sampleDatasets()
        {
            //int projectId = 6754;
            int length = Enum.GetNames(typeof(FieldConcreteDatasetEnum)).Length;


            string jsonString = await _concreteService.getSamples();
            var date = DateTime.Now;
            var dateString = date.Month.ToString() + "-" + date.Day.ToString() + "-" + date.Year.ToString();
            string fileName = "concreteDataExcel-" + dateString;

            return Ok(jsonString);
        }

        [Route("json")]
        [HttpGet]
        public async Task<IActionResult> GetConcreteJsonDatum(int projectId, int dataset)
        {

            FieldConcreteDatasetEnum dSet = (FieldConcreteDatasetEnum)dataset;
            string url = _concreteService.getUrl(projectId, dataset);

            

            if (dSet == FieldConcreteDatasetEnum.Full)
                return HandleResult(await _connectionService.concreteDatumData.OnGetData(url));
            else if (dSet == FieldConcreteDatasetEnum.Strength)
                return HandleResult(await _connectionService.concreteStrengthData.OnGetData(url));
            else if (dSet == FieldConcreteDatasetEnum.MixNumber)
                return HandleResult(await _connectionService.concreteMixNumberData.OnGetData(url));
            else
                return NotFound();
        }

        [Route("excel")]
        [HttpGet]
        public async Task<IActionResult> DownloadExcel(int projectId, int dataset)
        {
            FieldConcreteDatasetEnum dSet = (FieldConcreteDatasetEnum)dataset;
            string url = _concreteService.getUrl(projectId, dataset);

            byte[] ray = await _concreteService.createExcel(projectId, dataset);
            var date = DateTime.Now;
            var dateString = date.Month.ToString() + "-" + date.Day.ToString() + "-" + date.Year.ToString();
            string fileName = "concreteDataExcel-" + dateString;

            return File(
            fileContents: ray,
            contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",

            fileDownloadName: fileName + ".xlsx"
            );
        }
        [HttpGet]
        [Route("downloadJson")]
        public async Task<IActionResult> DownloadJson(int projectId, int dataset)
        {
            FieldConcreteDatasetEnum dSet = (FieldConcreteDatasetEnum)dataset;
            string url = _concreteService.getUrl(projectId, dataset);



            //var data = await _connectionService.concreteDatumData.OnGetData(url);

            if (dSet == FieldConcreteDatasetEnum.Full)
                return HandleJsonResult(await _connectionService.concreteDatumData.OnGetData(url));
            else if (dSet == FieldConcreteDatasetEnum.Strength)
                return HandleJsonResult(await _connectionService.concreteStrengthData.OnGetData(url));
            else if (dSet == FieldConcreteDatasetEnum.MixNumber)
                return HandleJsonResult(await _connectionService.concreteMixNumberData.OnGetData(url));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("downloadXml")]
        public async Task<IActionResult> DownloadXml(int projectId, int dataset)
        {
            FieldConcreteDatasetEnum dSet = (FieldConcreteDatasetEnum)dataset;
            string url = _concreteService.getUrl(projectId, dataset);

            byte[] ray = null;

            if (dSet == FieldConcreteDatasetEnum.Full)
            {
                var result = await _connectionService.concreteDatumData.OnGetData(url);
                ray = ConvertDataService.FromListToXml<FieldConcreteDatumDataset>(result.Value);
            }
            else if (dSet == FieldConcreteDatasetEnum.Strength)
            {
                var result = await _connectionService.concreteStrengthData.OnGetData(url);
                ray = ConvertDataService.FromListToXml<FieldConcreteStrengthDataset>(result.Value);
            }
            else if (dSet == FieldConcreteDatasetEnum.MixNumber)
            {
                var result = await _connectionService.concreteMixNumberData.OnGetData(url);
                ray = ConvertDataService.FromListToXml<FieldConcreteMixNumberDataset>(result.Value);

            }
            else
                return NotFound();

            var date = DateTime.Now;
            var dateString = date.Month.ToString() + "-" + date.Day.ToString() + "-" + date.Year.ToString();
            string fileName = "concreteDataXml-" + dateString;
            return File(ray, "application/xml", fileName + ".xml");
        }
    }
}