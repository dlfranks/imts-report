using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models.FieldConcreteTest;
using API.Services;
using API.Services.ConcreteService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class FieldConcreteTestController : BaseApiController
    {
        private readonly IConcreteService concreteService;
        private readonly string baseUrl = "http://localhost:6777/ReportService/FieldConcreteJsonData";

        public FieldConcreteTestController(IConcreteService concreteService)
        {
            this.concreteService = concreteService;
        }

        [Route("datum")]
        [HttpGet]
        public async Task<IActionResult> GetConcreteJsonDatum ()
        {
            int projectId = 6754;
            int format = 2;
            
            string url = $"{baseUrl}?projectId={projectId}&format={format}";

            var result = await concreteService.concreteDatumJson.OnGetData(url);

            return HandleResult(result);
        }
    }

    
}