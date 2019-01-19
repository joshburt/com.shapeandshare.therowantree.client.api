using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Dtos
{
    public partial class Feature
    {
        public int UserFeatureId { get; set; }
        public int UserId { get; set; }
        public int FeatureId { get; set; }

        public virtual FeatureType FeatureNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
