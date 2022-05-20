using System.ComponentModel.DataAnnotations;

namespace Domain.imts
{
    
    public class Employee
    {
        public int id { get; set; }
        public int mainOfficeId { get; set; }
        public virtual Office mainOffice { get; set; }
        public string userName { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }
        public bool isProjectManager { get; set; }
        public bool isEngineer { get; set; }
        public string windowsTimeZoneId { get; set; }
        public bool isSuperUser { get; set; }
        public bool active { get; set; }
    }


}