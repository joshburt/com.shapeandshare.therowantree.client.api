using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Dtos
{
    public partial class UserFeatureState
    {
        public int UserFeatureStateId { get; set; }
        public int UserId { get; set; }
        public int FeatureId { get; set; }
        public int FeatureIndex { get; set; }

        public virtual User User { get; set; }
    }
}
