using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            string fileName = "concreteData-" + dateString;

            return this.File(
            fileContents: ray,
            contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",

            // By setting a file download name the framework will
            // automatically add the attachment Content-Disposition header
            fileDownloadName: fileName + ".xlsx"
        );
        }




    }


}