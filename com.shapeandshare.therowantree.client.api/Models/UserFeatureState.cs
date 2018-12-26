using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class UserFeatureState
    {
        public int UserFeatureStateId { get; set; }
        public int UserId { get; set; }
        public int FeatureId { get; set; }
        public int FeatureIndex { get; set; }

        public User User { get; set; }
    }
}
