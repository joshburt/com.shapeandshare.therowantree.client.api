using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Dtos
{
    public partial class IncomeSource
    {
        public int IncomeSourceStoreId { get; set; }
        public int IncomeSourceId { get; set; }
        public int StoreId { get; set; }
        public float Amount { get; set; }

        public virtual IncomeSourceType IncomeSourceNavigation { get; set; }
        public virtual StoreType Store { get; set; }
    }
}
