using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class FeatureType
    {
        public FeatureType()
        {
            EventType = new HashSet<EventType>();
            Feature = new HashSet<Feature>();
            FeatureState = new HashSet<FeatureState>();
            UserGameState = new HashSet<UserGameState>();
        }

        public int FeatureId { get; set; }
        public string FeatureName { get; set; }

        public virtual ICollection<EventType> EventType { get; set; }
        public virtual ICollection<Feature> Feature { get; set; }
        public virtual ICollection<FeatureState> FeatureState { get; set; }
        public virtual ICollection<UserGameState> UserGameState { get; set; }
    }
}
