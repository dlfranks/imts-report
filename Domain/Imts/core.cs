namespace Domain.Imts
{
    public class Project
    {
        public int id { get; set; }

        public int officeId { get; set; }
        public virtual Office office { get; set; }
        public string projectNo { get; set; }
        public string name { get; set; }
    }

    public class Office{
        public int id { get; set; }
        public string name { get; set; }

        public int resLocaleId { get; set; }        
        public int unitSystem { get; set; }         
        public int region { get; set; }      
    }
}