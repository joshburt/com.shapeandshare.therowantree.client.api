using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class StoreType
    {
        public StoreType()
        {
            IncomeSource = new HashSet<IncomeSource>();
            MerchantTransformsFromStore = new HashSet<MerchantTransforms>();
            MerchantTransformsToStore = new HashSet<MerchantTransforms>();
            Store = new HashSet<Store>();
        }

        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreDescription { get; set; }

        public Trapdrop Trapdrop { get; set; }
        public ICollection<IncomeSource> IncomeSource { get; set; }
        public ICollection<MerchantTransforms> MerchantTransformsFromStore { get; set; }
        public ICollection<MerchantTransforms> MerchantTransformsToStore { get; set; }
        public ICollection<Store> Store { get; set; }
    }
}
