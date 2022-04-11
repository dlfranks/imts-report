using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using API.Helper;
using API.Models.Core;

namespace API.Models.FieldConcreteTest
{
    public enum FieldConcreteDatasetEnum
    {
        Full = 1,
        Strength,
        MixNumber,

    }
    public abstract class FieldConcreteSampleDataset
    {

        public List<FieldConcreteDatumFlattenDataset> fullDataset { get; set; }
        public List<FieldConcreteStrengthDataset> strengthDataset { get; set; }
        public List<FieldConcreteMixNumberDataset> mixNumberDataset { get; set; }


    }

    // public class ConcreteFullFactory : ConcreteDatumFactory
    // {
    //     private ConnectionService _connectionService;
    //     public ConcreteFullFactory(ConnectionService connectionService){
    //         _connectionService = connectionService;
    //     }
    //     public override async Task<List<FieldConcreteDatumDataset>> getService(string url)
    //     {
    //         Result<List<FieldConcreteDatumDataset>> result = await _connectionService.concreteDatumData.OnGetData(url);
    //         return result.Value;
    //     }

    // }
    // public class ConcreteStrengthFactory : ConcreteDatumFactory
    // {
    //     private ConnectionService _connectionService;
    //     public ConcreteStrengthFactory(ConnectionService connectionService){
    //         _connectionService = connectionService;
    //     }
    //     public override async Task<List<FieldConcreteDatum>> getService(string url)
    //     {
    //         Result<List<FieldConcreteStrengthDataset>> result = await _connectionService.concreteStrengthData.OnGetData(url);
    //         List<FieldConcreteDatum> dataset = (List<FieldConcreteDatum>)result.Value;
    //         return dataset;
    //     }

    // }
    // public abstract class ConcreteDatumFactory{

    //     public abstract Task<List<FieldConcreteDatum>> getService(string url);

    // }

    public class FieldConcreteProps
    {
        public object fields { get; set; }
        public int length { get; set; }

    }
    public class FieldConcreteBase
    {

        public string projectNo { get; set; }
        public string projectName { get; set; }
        public string labNo { get; set; }
        public DateTime? castDate { get; set; }
        protected async Task<Result<T>> OnGetData<T>(string url)
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get,
                url);
            request.Headers.Add("Accept", "application/json");

            var response = await client.SendAsync(request);
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
            };
            options.Converters.Add(new TimeSpanToStringConverter());

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                T data = await JsonSerializer.DeserializeAsync<T>(responseStream, options);
                return Result<T>.Success(data);
            }
            else
            {
                return Result<T>.Failure("HttpClient Failed.");
            }
        }
    }
    public class FieldConcreteDatumAttachmentDataset
    {
        public string originalFilename { get; set; }
        public string officeAssetFileName { get; set; }

    }
    public class FieldConcreteDatumStrengthRowDataset
    {
        public double strength { get; set; }
        public int days { get; set; }
        public string displayStrengthType { get; set; }
    }
    public class FieldConcreteDatumBatchRowDataset
    {
        public string name { get; set; }
        public double weight { get; set; }
        public double moisture { get; set; }

    }
    public class FieldConcreteDatumTestRowDataset
    {
        public int id { get; set; }
        public string specimenId { get; set; }
        public int daysToAge { get; set; }
        public DateTime? receiveDate { get; set; }
        public TimeSpan? demoldTime { get; set; }
        public DateTime? transportDate { get; set; }
        public TimeSpan? transportTime { get; set; }
        public DateTime? testedOnDate { get; set; }
        public double widthDiameter { get; set; }
        public double heightLength { get; set; }
        public double? span { get; set; }
        public double? testLoad { get; set; }
        public double? calcCorrectionFactor { get; set; }
        public double? correctionFactorOverride { get; set; }
        public double? calcArea { get; set; }
        public double? calcCompressiveStrength { get; set; }
        public double? calcCompressiveStrengthUnrounded { get; set; }
        public string displayResFractureType { get; set; }
        public string displayResCapType { get; set; }
        public bool wasFieldCured { get; set; }
        public string displayTechnicianName { get; set; }
        public double? specimenWeight { get; set; }
        public double? breakDist { get; set; }
        public double? flexuralBeamLength { get; set; }
        public double? calcFlexuralBeamUnitWeight { get; set; }
        public string curingLocation { get; set; }
        //Break Machine
        public string equipmentName { get; set; }
        //Density
        public double? receivedAirWeight { get; set; }
        public double? testedAirWeight { get; set; }
        public double? testedWaterWeight { get; set; }
        public double? densityWater { get; set; }
        public double? calcDensity { get; set; }
        public bool isComputedWithDiaLength { get; set; }
        public bool isWeightWithAirWater { get; set; }
        public bool showCalulationsForMasses { get; set; }
        //Calculate density fields
        public double? calcWaterContent { get; set; }
        public double? calcAbsAfterImmersion { get; set; }
        public double? calcAbsAfterImmersionBoiling { get; set; }
        public double? calcAirDryOrEqDensity { get; set; }
        public double? calcDryDensity { get; set; }
        public double? calcDensityImmersion { get; set; }
        public double? calcDensityImmersionBoiling { get; set; }
        public double? calcVolumePermeablePore { get; set; }

    }

    public class FieldConcreteDatumDataset : FieldConcreteBase
    {
        public FieldConcreteDatumDataset()
        {
            this.batchRows = new List<FieldConcreteDatumBatchRowDataset>();
            this.strengthRows = new List<FieldConcreteDatumStrengthRowDataset>();
            this.attachments = new List<FieldConcreteDatumAttachmentDataset>();
            this.testRows = new List<FieldConcreteDatumTestRowDataset>();
        }
        //testSubTypes based on region, ie CSA or ASTM/AAHSTO depends on officeRegion, gram weight of specimens is only CSA
        //----Sample-------------------------------------
        public string displayWorkflowStateName { get; set; }
        public bool displayFieldConcreteAllBreaksCompleted { get; set; }
        //-----Test-----------------------------------------
        public string displayPhase { get; set; }   //Eventually id's
        public string displayTask { get; set; }
        public string displayAssignedTo { get; set; }
        public string displayCompletedBy { get; set; }
        public string displayVerifiedBy { get; set; }
        public string displayWorkflowState { get; set; }
        public string displayspecGroup { get; set; }
        //---Field Signature----------------
        public string displayFieldSignerName { get; set; }
        public string displayFieldSignerEmail { get; set; }
        //Audit
        public string displayCreatedAuditName { get; set; }
        public DateTime? displayCreatedAuditDate { get; set; }
        public string displayLastUpdatedAuditName { get; set; }
        public DateTime? displayLastUpdatedAuditDate { get; set; }

        //-----Project---------------------------------------------
        public string displayOfficeName { get; set; }
        public string displayAddress { get; set; }
        public string displayProjectManaer { get; set; }
        public bool DisplayIsEngineerSignatureRequired { get; set; }
        //Client and client contact
        public string displayClientName { get; set; }
        //Contractor
        public string displayContractorName { get; set; }
        //Supplier
        public string displaySupplierName { get; set; }
        public string displaySupplierEmail { get; set; }
        //-- Entity Data --------------------------------------------------------------
        public string displayUnitSystem { get; set; }
        //Sub Test
        public string displayTestSubTypeName { get; set; }
        // Test Procedure
        public string displayResTestProcedureName { get; set; }
        // Test Standard
        public string displayResTestStandardName { get; set; }
        //Mix Number
        public string mixNumber { get; set; }
        //Inspector
        public string displayInspectorName { get; set; }
        public string inspector2 { get; set; }
        //FieldConcreteUnitWeightRow
        //Slump
        public double? displayUwSlumpActual { get; set; }
        public double? displayUwSlumpAfterSP { get; set; }
        public double? displayUwSlumpSpecMin { get; set; }
        public double? displayUwSlumpSpecMax { get; set; }
        //Spread
        public double? displayUwSpreadActual { get; set; }
        public double? displayUwSpreadAfterSP { get; set; }
        public double? displayUwSpreadSpecMin { get; set; }
        public double? displayUwSpreadSpecMax { get; set; }
        //Air
        public double? displayUwAirActual { get; set; }
        public double? displayUwAirAfterSP { get; set; }
        public double? displayUwAirSpecMin { get; set; }
        public double? displayUwAirSpecMax { get; set; }
        //Weight
        public double? displayUwWeightActual { get; set; }
        public double? displayUwWeightAfterSP { get; set; }
        public double? displayUwWeightSpecMin { get; set; }
        public double? displayUwWeightSpecMax { get; set; }
        //ConcreteTemp
        public double? displayUwConcreteTempActual { get; set; }
        public double? displayUwConcreteTempAfterSP { get; set; }
        public double? displayUwConcreteTempSpecMin { get; set; }
        public double? displayUwConcreteTempSpecMax { get; set; }
        //Unit Weight Data
        public double? weightWithConcreteSP { get; set; }
        public double? weightWithConcrete { get; set; }
        public double? weightEmpty { get; set; }
        public double? volumeOfMeasure { get; set; }
        public bool wasAggregateCorrectionDone { get; set; }
        public int? flexuralBeamTestType { get; set; }
        public string permitNo { get; set; }
        public string placementType { get; set; }
        public string placementLocation { get; set; }
        public string specimenStorage { get; set; }
        public string notes { get; set; }
        public int? truckLoadNum { get; set; }
        public bool truckLoadNumNA { get; set; }
        public int? ofTotalTruckLoads { get; set; }
        public string truckNumber { get; set; }
        public string ticketNumber { get; set; }
        public string plantNumber { get; set; }
        public string cementType { get; set; }
        public string moldType { get; set; }
        public double? batchSize { get; set; }
        public bool LoadBatchVolumneNA { get; set; }
        public double? cumulativeLoadVolume { get; set; }
        public string loadVolumeUnits { get; set; }
        public TimeSpan? batchTime { get; set; }
        public TimeSpan? sampleTime { get; set; }
        public TimeSpan? finishTime { get; set; }
        public double? waterAdded { get; set; }
        public bool waterAddedNA { get; set; }
        public string adMixture { get; set; }
        public int? resCloudType { get; set; }
        public int? resPrecipitationType { get; set; }
        public int? resWindType { get; set; }
        public bool isEmailed { get; set; }
        public double? hiTemp { get; set; }
        public double? lowTemp { get; set; }
        public string initialCuringCondition { get; set; }
        public int? resFinalCure { get; set; }
        public bool testDoneInternally { get; set; }
        public string revolution { get; set; }
        public string truckDischarge { get; set; }
        public string sampledFrom { get; set; }
        public string clientNotification { get; set; }

        //Batch Data
        public double? cementQuantity { get; set; }
        public double? flyAshQuantity { get; set; }
        public double? waterQuantity { get; set; }
        public int? resWaterUnits { get; set; }
        public double? calcYield { get; set; }
        public double? calcWCRatio { get; set; }
        public double? calcCementContent { get; set; }
        public bool isNoMoistureGiven { get; set; }
        //billing items
        public double? travelHours { get; set; }
        public double? testHours { get; set; }
        public double? retestHours { get; set; }
        public double? standbyHours { get; set; }
        public double? calcTotalHours { get; set; }
        public double? mileage { get; set; }
        //Lists
        public virtual List<FieldConcreteDatumBatchRowDataset> batchRows { get; set; }
        //Strengths
        public virtual List<FieldConcreteDatumStrengthRowDataset> strengthRows { get; set; }
        //Attachments
        public virtual List<FieldConcreteDatumAttachmentDataset> attachments { get; set; }
        //Specimens
        public virtual List<FieldConcreteDatumTestRowDataset> testRows { get; set; }
    }

    public class FieldConcreteDatumFlattenDataset : FieldConcreteBase
    {
        //----Sample-------------------------------------
        public string displayWorkflowStateName { get; set; }
        public bool displayFieldConcreteAllBreaksCompleted { get; set; }
        //-----Test-----------------------------------------
        public string displayPhase { get; set; }   //Eventually id's
        public string displayTask { get; set; }
        public string displayAssignedTo { get; set; }
        public string displayCompletedBy { get; set; }
        public string displayVerifiedBy { get; set; }
        public string displayWorkflowState { get; set; }
        public string displayspecGroup { get; set; }
        //---Field Signature----------------
        public string displayFieldSignerName { get; set; }
        public string displayFieldSignerEmail { get; set; }
        //Audit
        public string displayCreatedAuditName { get; set; }
        public DateTime? displayCreatedAuditDate { get; set; }
        public string displayLastUpdatedAuditName { get; set; }
        public DateTime? displayLastUpdatedAuditDate { get; set; }

        //-----Project---------------------------------------------
        public string displayOfficeName { get; set; }
        public string displayAddress { get; set; }
        public string displayProjectManaer { get; set; }
        public bool DisplayIsEngineerSignatureRequired { get; set; }
        //Client and client contact
        public string displayClientName { get; set; }
        //Contractor
        public string displayContractorName { get; set; }
        //Supplier
        public string displaySupplierName { get; set; }
        public string displaySupplierEmail { get; set; }
        //-- Entity Data --------------------------------------------------------------
        public string displayUnitSystem { get; set; }
        //Sub Test
        public string displayTestSubTypeName { get; set; }
        // Test Procedure
        public string displayResTestProcedureName { get; set; }
        // Test Standard
        public string displayResTestStandardName { get; set; }
        //Mix Number
        public string mixNumber { get; set; }
        //Inspector
        public string displayInspectorName { get; set; }
        public string inspector2 { get; set; }
        //FieldConcreteUnitWeightRow
        //Slump
        public double? displayUwSlumpActual { get; set; }
        public double? displayUwSlumpAfterSP { get; set; }
        public double? displayUwSlumpSpecMin { get; set; }
        public double? displayUwSlumpSpecMax { get; set; }
        //Spread
        public double? displayUwSpreadActual { get; set; }
        public double? displayUwSpreadAfterSP { get; set; }
        public double? displayUwSpreadSpecMin { get; set; }
        public double? displayUwSpreadSpecMax { get; set; }
        //Air
        public double? displayUwAirActual { get; set; }
        public double? displayUwAirAfterSP { get; set; }
        public double? displayUwAirSpecMin { get; set; }
        public double? displayUwAirSpecMax { get; set; }
        //Weight
        public double? displayUwWeightActual { get; set; }
        public double? displayUwWeightAfterSP { get; set; }
        public double? displayUwWeightSpecMin { get; set; }
        public double? displayUwWeightSpecMax { get; set; }
        //ConcreteTemp
        public double? displayUwConcreteTempActual { get; set; }
        public double? displayUwConcreteTempAfterSP { get; set; }
        public double? displayUwConcreteTempSpecMin { get; set; }
        public double? displayUwConcreteTempSpecMax { get; set; }
        //Unit Weight Data
        public double? weightWithConcreteSP { get; set; }
        public double? weightWithConcrete { get; set; }
        public double? weightEmpty { get; set; }
        public double? volumeOfMeasure { get; set; }
        public bool wasAggregateCorrectionDone { get; set; }
        public int? flexuralBeamTestType { get; set; }
        public string permitNo { get; set; }
        public string placementType { get; set; }
        public string placementLocation { get; set; }
        public string specimenStorage { get; set; }
        public string notes { get; set; }
        public int? truckLoadNum { get; set; }
        public bool truckLoadNumNA { get; set; }
        public int? ofTotalTruckLoads { get; set; }
        public string truckNumber { get; set; }
        public string ticketNumber { get; set; }
        public string plantNumber { get; set; }
        public string cementType { get; set; }
        public string moldType { get; set; }
        public double? batchSize { get; set; }
        public bool LoadBatchVolumneNA { get; set; }
        public double? cumulativeLoadVolume { get; set; }
        public string loadVolumeUnits { get; set; }
        public TimeSpan? batchTime { get; set; }
        public TimeSpan? sampleTime { get; set; }
        public TimeSpan? finishTime { get; set; }
        public double? waterAdded { get; set; }
        public bool waterAddedNA { get; set; }
        public string adMixture { get; set; }
        public int? resCloudType { get; set; }
        public int? resPrecipitationType { get; set; }
        public int? resWindType { get; set; }
        public bool isEmailed { get; set; }
        public double? hiTemp { get; set; }
        public double? lowTemp { get; set; }
        public string initialCuringCondition { get; set; }
        public int? resFinalCure { get; set; }
        public bool testDoneInternally { get; set; }
        public string revolution { get; set; }
        public string truckDischarge { get; set; }
        public string sampledFrom { get; set; }
        public string clientNotification { get; set; }

        //Batch Data
        public double? cementQuantity { get; set; }
        public double? flyAshQuantity { get; set; }
        public double? waterQuantity { get; set; }
        public int? resWaterUnits { get; set; }
        public double? calcYield { get; set; }
        public double? calcWCRatio { get; set; }
        public double? calcCementContent { get; set; }
        public bool isNoMoistureGiven { get; set; }
        //billing items
        public double? travelHours { get; set; }
        public double? testHours { get; set; }
        public double? retestHours { get; set; }
        public double? standbyHours { get; set; }
        public double? calcTotalHours { get; set; }
        public double? mileage { get; set; }
        public double strengthRow_strength_0 { get; set; }
        public int strengthRow_days_0 { get; set; }
        public string strengthRow_strengthType_0 { get; set; }
        public virtual List<FieldConcreteDatumTestRowDataset> testRows { get; set; }
    }

    public class FieldConcreteStrengthDataset : FieldConcreteBase
    {
        public FieldConcreteStrengthDataset()
        {
            testRows = new List<FieldConcreteStrengthRowDataset>();
        }
        public string displayTestSubTypeName { get; set; }
        public string displaySupplierName { get; set; }
        public string mixNumber { get; set; }
        public double? displayUwSlumpActual { get; set; }
        public double? displayUwSpreadActual { get; set; }
        public double? displayUwAirActual { get; set; }
        public double? displayUwWeightActual { get; set; }
        public double? displayUwConcreteTempActual { get; set; }
        public List<FieldConcreteStrengthRowDataset> testRows { get; set; }
    }
    //public class FieldConcreteSpecimenRowDataset
    //{
    //    public int id { get; set; }

    //    public double? receivedAirWeight { get; set; }

    //    public double? ovenDriedSpecimen { get; set; }

    //    public double? surfaceDriedAirImmersion { get; set; }

    //    public double? surfaceDriedAirImmersionBoiling { get; set; }

    //    public double? specimenWaterImmersionBoiling { get; set; }

    //    public double? specimenReachingEquilibrium { get; set; }
    //}
    public class FieldConcreteStrengthRowDataset
    {
        public string specimenId { get; set; }
        public int daysToAge { get; set; }
        public DateTime? testedOnDate { get; set; }
        public double widthDiameter { get; set; }
        public double heightLength { get; set; }
        public double? testLoad { get; set; }
        public double? calcArea { get; set; }
        public double? calcCompressiveStrength { get; set; }
        public double? calcCompressiveStrengthUnrounded { get; set; }
    }
    public class FieldConcreteMixNumberDataset : FieldConcreteBase
    {
        public FieldConcreteMixNumberDataset()
        {
            testRows = new List<FieldConcreteMixNumberRowDataset>();
        }

        public string mixNumber { get; set; }
        public List<FieldConcreteMixNumberRowDataset> testRows { get; set; }
    }

    public class FieldConcreteMixNumberRowDataset
    {
        public string specimenId { get; set; }
        public int daysToAge { get; set; }
    }

}