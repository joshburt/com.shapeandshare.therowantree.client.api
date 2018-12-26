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

        public ICollection<EventType> EventType { get; set; }
        public ICollection<Feature> Feature { get; set; }
        public ICollection<FeatureState> FeatureState { get; set; }
        public ICollection<UserGameState> UserGameState { get; set; }
    }
}
