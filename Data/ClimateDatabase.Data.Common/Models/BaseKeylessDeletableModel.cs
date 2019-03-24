namespace ClimateDatabase.Data.Common.Models
{
    using System;

    public abstract class BaseKeylessDeletableModel : BaseKeylessModel, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
