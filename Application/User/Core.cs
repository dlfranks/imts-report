using System.ComponentModel.DataAnnotations;
using Domain;

namespace Application.User
{
    public class UserOfficeRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }

    public class UsersInOfficeRole
    {
        public int UserRoleId { get; set; }
        public UserOfficeRole UserRole { get; set; }
        public int AppUserId { get; set; }
        public AppUser user { get; set; }
        //Users have roles by office
        public int officeId { get; set; }
        public Office office { get; set; }
    }
    public class Office
    {
        public int id { get; set; }
        public string name { get; set; }

        public int resLocaleId { get; set; }        //Enum LocaleEnum
        public int unitSystem { get; set; }         //Enum UnitSystem
        public int region { get; set; }             //Enum OfficeRegion

        [Display(Name = "Report Footer Line 1")]
        public string reportFooterLine1 { get; set; }
        [Display(Name = "Report Footer Line 2")]
        public string reportFooterLine2 { get; set; }

        //Canadian only CCil Certifications.  These are configured on the office administration page.
        //When configured, they can cause a CCiL logo and text to appear on client reports.
        public bool? isCcilAsphaltCertification { get; set; }
        public string ccilAsphaltCertificationText { get; set; }
        public bool? isCcilAggregatesCertification { get; set; }
        public string ccilAggregatesCertificationText { get; set; }
        public bool? isCcilConcreteCertification { get; set; }
        public string ccilConcreteCertificationText { get; set; }

        //Defaults and configuration------------------------------------------------------------
        //Sieve Settings
        public string cfgdef_sieve_defaultSieveString { get; set; }

        //Concrete Settings
        public int cfgdef_fieldConcrete_automaticPickupDispatch { get; set; }  //Enum AutomaticConcretePickupDispatchConfig
        //Vogtle
        public int? cfgdef_fieldConcrete_lengthDecimalPlaces { get; set; }
        public int? cfgdef_fieldConcrete_diameterDecimalPlaces { get; set; }
        public int? cfgdef_fieldConcrete_widthDecimalPlaces { get; set; }
        public int? cfgdef_fieldConcrete_heightDecimalPlaces { get; set; }
        public int? cfgdef_fieldConcrete_areaDecimalPlaces { get; set; }

        //Field density settings
        public int cfgdef_fieldDensityAsphalt_compactionDecimalPlaces { get; set; }
        public bool cfgdef_fieldDensityAsphalt_useBulkUW { get; set; }
        public int cfgdef_fieldDensitySoil_compactionDecimalPlaces { get; set; }
        //Asphalt Lab settings
        public double cfgdef_riceMarshallGyratory_WaterDensity { get; set; }
        //Dispatch and Report settings
        public bool cfgdef_dispatchReportTrackingEmail { get; set; }
        public bool cfgdef_dispatchReportRecordTieMandatory { get; set; }
    }
}