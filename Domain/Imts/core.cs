namespace Domain.imts
{
    public class IDValuePair
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public enum UnitSystem
    {
        Standard = 0,
        Metric = 1
    }

    //Affects what options are available in certain tests and screens
    public enum OfficeRegion
    {
        US = 0,
        Canada = 1
    }
    public class UserOfficeRole
    {
        public int id { get; set; }
        public string roleName { get; set; }
    }
    public class Office
    {
        public int id { get; set; }
        public string name { get; set; }
        public int unitSystem { get; set; }         //Enum UnitSystem
        public int region { get; set; }             //Enum OfficeRegion
    }

    public class UsersInOfficeRole
    {
        public int userRoleId { get; set; }
        public virtual UserOfficeRole userRole { get; set; }
        public int employeeId { get; set; }
        public virtual Employee employee { get; set; }
        //Users have roles by office
        public int officeId { get; set; }
        public virtual Office office { get; set; }
    }
    
    public class Project
    {
        public int id { get; set; }

        public int officeId { get; set; }
        public virtual Office office { get; set; }
        public string projectNo { get; set; }
        public string name { get; set; }
    }



}