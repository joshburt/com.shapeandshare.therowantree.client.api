using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Dtos
{
    public partial class MerchantTransforms
    {
        public int MerchantTransformsId { get; set; }
        public int ToStoreId { get; set; }
        public int FromStoreId { get; set; }
        public int Amount { get; set; }

        public virtual StoreType FromStore { get; set; }
        public virtual StoreType ToStore { get; set; }
    }
}
