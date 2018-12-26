using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class UserGameState
    {
        public int UserId { get; set; }
        public int ActiveFeature { get; set; }
        public int GameTemperatureId { get; set; }
        public int GameFireStateId { get; set; }
        public int BuilderLevel { get; set; }

        public FeatureType ActiveFeatureNavigation { get; set; }
        public FireType GameFireState { get; set; }
        public TemperatureType GameTemperature { get; set; }
    }
}
