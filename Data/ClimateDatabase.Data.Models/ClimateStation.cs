namespace ClimateDatabase.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using ClimateDatabase.Data.Common.Models;

    public class ClimateStation : BaseDeletableModel<string>
    {
        public ClimateStation()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public virtual ICollection<ClimateStationReading> Readings { get; set; }
    }
}
