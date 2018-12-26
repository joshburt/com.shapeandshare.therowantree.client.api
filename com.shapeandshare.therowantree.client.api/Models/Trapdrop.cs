using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class Trapdrop
    {
        public int StoreId { get; set; }
        public double RollUnder { get; set; }
        public string Message { get; set; }

        public virtual StoreType Store { get; set; }
    }
}
