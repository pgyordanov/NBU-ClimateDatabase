﻿namespace ClimateDatabase.Web.Areas.Admin.Models.ClimateStationWeight
{
    using System.Collections.Generic;

    using ClimateDatabase.Data.Models;
    using ClimateDatabase.Web.Areas.Admin.Models.Base;

    public class ClimateStationReadingWeightListVM
    {
       public List<ClimateStationReadingWeightVM> ClimateStationReadingWeights { get; set; }
    }
}
