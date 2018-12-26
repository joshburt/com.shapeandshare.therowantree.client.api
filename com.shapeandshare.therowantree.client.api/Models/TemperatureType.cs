using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class TemperatureType
    {
        public TemperatureType()
        {
            UserGameState = new HashSet<UserGameState>();
        }

        public int TemperatureId { get; set; }
        public string TemperatureName { get; set; }
        public string TemperatureDescription { get; set; }

        public ICollection<UserGameState> UserGameState { get; set; }
    }
}
