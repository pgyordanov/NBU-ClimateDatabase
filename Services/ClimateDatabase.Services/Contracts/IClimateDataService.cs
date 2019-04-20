namespace ClimateDatabase.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ClimateDatabase.Services.Models;

    public interface IClimateDataService
    {
        Dictionary<string, double> GetWeightedDataForPeriodByField(ClimateDataFilter filter);
        
        List<ClimateDataModelByDate> GetWeightedClimateDataForPeriodFull(ClimateDataFilter filter);
        
        Task<List<ClimateDataModelByStation>> GetClimateDataByStation(ClimateDataFilter filter);
    }
}