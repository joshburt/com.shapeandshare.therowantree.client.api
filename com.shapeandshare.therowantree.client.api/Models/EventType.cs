using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class EventType
    {
        public int EventId { get; set; }
        public int ActiveFeatureId { get; set; }
        public int FeatureStateId { get; set; }
        public string EventName { get; set; }
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }

        public virtual FeatureType ActiveFeature { get; set; }
        public virtual FeatureState ActiveFeatureNavigation { get; set; }
    }
}
