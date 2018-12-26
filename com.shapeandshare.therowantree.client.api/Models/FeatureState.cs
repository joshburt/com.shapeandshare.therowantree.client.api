using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class FeatureState
    {
        public FeatureState()
        {
            EventType = new HashSet<EventType>();
        }

        public int FeatureStateId { get; set; }
        public int FeatureId { get; set; }
        public int StateIndex { get; set; }
        public string StateName { get; set; }
        public string StateDescription { get; set; }

        public FeatureType Feature { get; set; }
        public ICollection<EventType> EventType { get; set; }
    }
}
