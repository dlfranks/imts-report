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
        private readonly IConcreteService concreteService;
        private readonly string baseUrl = "http://localhost:6777/ReportService/FieldConcreteJsonData";
        private readonly ImtsContext _imtsContext;

        public FieldConcreteTestController(IConcreteService concreteService, ImtsContext imtsContext)
        {
            _imtsContext = imtsContext;
            this.concreteService = concreteService;
        }

        [Route("datum")]
        [HttpGet]
        public async Task<IActionResult> GetConcreteJsonDatum(int projectId)
        {
            projectId = 6754;
            int format = 2;

            string url = $"{baseUrl}?projectId={projectId}&format={format}";

            var result = await concreteService.concreteDatumJson.OnGetData(url);

            return HandleResult(result);
        }

        [Route("project")]
        [HttpPost]
        public async Task<IActionResult> ProjectAutoComplete()
        {
            var result = await _imtsContext.Projects.Include("office").ToListAsync();
            //var result = await Mediator.Send(new ProjectList.Query());
            return Ok(result);
        }
    }


}