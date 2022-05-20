using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.imts;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Imts
{
    public class ImtsUserService
    {
        private readonly ImtsContext _imtsContext;
        public ImtsUserService(ImtsContext imtsContext)
        {
            _imtsContext = imtsContext;
        }
        public async Task<Office> GetOfficeFromImts(int officeId)
        {
            return await _imtsContext.Offices.Where(q => q.id == officeId).FirstAsync();
        }
        public async Task<List<IDValuePair>> GetUserOfficesFromImts(int employeeId)
        {
            var list = await _imtsContext.UsersInOfficeRoles.Include(q => q.office)
            .Where(q => q.employeeId == employeeId).GroupBy(q => q.officeId).Select(g => g.First().office).ToListAsync();
            var lst = list.Select(u => new IDValuePair() { id = u.id, name = u.name }).ToList();

            return lst;
        }
        public async Task<Employee> GetImtsUserById(int employeeId)
        {
            Employee employee = await _imtsContext.Employees.Where(q => q.id == employeeId && q.active == true).FirstOrDefaultAsync();
            if (employee == null) return null;
            return employee;
        }
        public async Task<bool> IsImtsUser(string userName)
        {
            var imtsUser = await GetImtsUserByUserName(userName);
            if (imtsUser != null)
                return true;
            else return false;
        }

        public async Task<Employee> GetImtsUserByUserName(string userName)
        {

            userName = userName.ToLower();

            //If an email was specified, trim it for amec/amecfw users.  All other users, leave it on there.
            var emin = userName.IndexOf('@');
            if (emin > -1)
            {
                var em = userName.Substring(emin + 1);
                if (em.ToLower() == "amec.com" || em.ToLower() == "amecfw.com" || em.ToLower() == "woodplc.com")
                {
                    userName = userName.Substring(0, emin);
                }
            }

            //If no domain was specified and it is an internal address, then
            //autopopulate the domain for situations where there is only one user
            userName = userName.Replace('/', '\\');
            if (userName.IndexOf('\\') == -1 && userName.IndexOf('@') == -1)
            {
                var e = _imtsContext.Employees.Where(q => q.userName == userName && q.active == true);
                //Straight username match
                if (e.Count() == 0)
                {
                    //Pick domain account if only one (more than one you need to specify)
                    var tmp = "\\" + userName;
                    e = _imtsContext.Employees.Where(q => q.userName.EndsWith(tmp));
                    if (e.Count() == 1)
                    {
                        var result = await e.FirstAsync();
                        userName = result.userName;
                    }
                }
            }
            Employee employee = await _imtsContext.Employees.Where(q => q.userName == userName && q.active == true).FirstOrDefaultAsync();
            if (employee != null)
                return employee;
            else
                return null;

        }
        public async Task<string> GetImtsUserName(string userName)
        {
            var employee = await _imtsContext.Employees.Where(q => q.userName == userName).FirstOrDefaultAsync();
            if (employee == null)
                return employee.userName;
            else
                return "";
        }
    }
}