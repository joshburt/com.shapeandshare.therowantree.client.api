using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.trt
{
    public partial class StoreType
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreDescription { get; set; }

        public Trapdrop Trapdrop { get; set; }
    }
}
