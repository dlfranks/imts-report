
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ProjectController : BaseApiController
    {
        private readonly ImtsContext _imtsContext;
        public ProjectController(ImtsContext imtsContext)
        {
            _imtsContext = imtsContext;
        }

        [Route("autocomplete")]
        [HttpGet]
        public async Task<IActionResult> ProjectAutoComplete(int officeId, string term)
        {
            var result = await _imtsContext.Projects
                .Include("office")
                .Where(q => q.office.id == officeId && q.name.Contains(term))
                .Select(q => new ProjectViewModel{
                    Id=q.id,
                    Number = q.projectNo,
                    Name = q.name,
                    officeId = q.officeId
                })
                .Take(10)
                .ToListAsync();
            
            return Ok(result);
        }
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> ProjectAutoComplete(int officeId)
        {
            var result = await _imtsContext.Projects
                .Include("office")
                .Where(q => q.office.id == officeId)
                .Select(q => new ProjectViewModel{
                    Id=q.id,
                    Number = q.projectNo,
                    Name = q.name,
                    officeId = q.officeId
                })
                .ToListAsync();
            
            return Ok(result);
        }
    }
}