using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using API.Models.FieldConcreteTest;
using API.Services;
using API.Services.ConcreteService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{

    public class FieldConcreteTestController : BaseApiController
    {
        private readonly IConnectionService _connectionService;


        private readonly ConcreteService _concreteService;

        public FieldConcreteTestController(IConnectionService connectionService, ConcreteService concreteService)
        {
            _concreteService = concreteService;

            _connectionService = connectionService;
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

            // By setting a file download name the framework will
            // automatically add the attachment Content-Disposition header
            fileDownloadName: fileName + ".xlsx"
            );
        }
        [HttpGet]
        [Route("downloadJson")]
        public async Task<IActionResult> DownloadJson(int projectId, int dataset)
        {
            FieldConcreteDatasetEnum dSet = (FieldConcreteDatasetEnum)dataset;
            string url = _concreteService.getUrl(projectId, dataset);

            dynamic ray = null;

            if (dSet == FieldConcreteDatasetEnum.Full)
                ray = await _connectionService.concreteDatumData.OnGetData(url);
            else if (dSet == FieldConcreteDatasetEnum.Strength)
                ray = await _connectionService.concreteStrengthData.OnGetData(url);
            else if (dSet == FieldConcreteDatasetEnum.MixNumber)
                ray = await _connectionService.concreteMixNumberData.OnGetData(url);
            else
                return NotFound();


            string jsonString = JsonSerializer.Serialize(ray);
            var date = DateTime.Now;
            var dateString = date.Month.ToString() + "-" + date.Day.ToString() + "-" + date.Year.ToString();
            string fileName = "concreteDataJson-" + dateString;

            HttpContext.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            HttpContext.Response.Headers.Add("content-disposition", "attachment; filename=" + fileName + ".json");
            return new JsonResult(ray

        );
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
            }else if (dSet == FieldConcreteDatasetEnum.Strength)
            {
                var result =  await _connectionService.concreteStrengthData.OnGetData(url);
                ray = ConvertDataService.FromListToXml<FieldConcreteStrengthDataset>(result.Value);
            }else if (dSet == FieldConcreteDatasetEnum.MixNumber)
            {
                var result = await _connectionService.concreteMixNumberData.OnGetData(url);
                ray = ConvertDataService.FromListToXml<FieldConcreteMixNumberDataset>(result.Value);

            }else
                return NotFound();
            
                
               
            // else if (dSet == FieldConcreteDatasetEnum.Strength)
            //     ray = await _connectionService.concreteStrengthData.OnGetData(url);
            // else if (dSet == FieldConcreteDatasetEnum.MixNumber)
            //     ray = await _connectionService.concreteMixNumberData.OnGetData(url);
            // else
            //     return NotFound();

            
            
            
            var date = DateTime.Now;
            var dateString = date.Month.ToString() + "-" + date.Day.ToString() + "-" + date.Year.ToString();
            string fileName = "concreteDataXml-" + dateString;

            
            
            return File(ray, "application/xml", fileName + ".xml");
        }



    }


}