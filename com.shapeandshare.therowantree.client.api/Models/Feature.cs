using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class Feature
    {
        public int UserFeatureId { get; set; }
        public int UserId { get; set; }
        public int FeatureId { get; set; }

        public FeatureType FeatureNavigation { get; set; }
        public User User { get; set; }
    }
}
