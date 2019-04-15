namespace ClimateDatabase.Services.Contracts
{
    using System.Collections.Generic;
    using ClimateDatabase.Services.Models;

    public interface IClimateDataService
    {
        Dictionary<string, double> GetClimateDataForPeriodByField(ClimateDataFilter filter);
    }
}