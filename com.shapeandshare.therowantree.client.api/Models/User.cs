using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class User
    {
        public User()
        {
            Feature = new HashSet<Feature>();
            Perk = new HashSet<Perk>();
            Store = new HashSet<Store>();
            UserFeatureState = new HashSet<UserFeatureState>();
            UserIncome = new HashSet<UserIncome>();
            UserNotification = new HashSet<UserNotification>();
        }

        public int UserId { get; set; }
        public string Guid { get; set; }
        public byte? Active { get; set; }
        public int Population { get; set; }

        public virtual UserInfo UserInfo { get; set; }
        public virtual ICollection<Feature> Feature { get; set; }
        public virtual ICollection<Perk> Perk { get; set; }
        public virtual ICollection<Store> Store { get; set; }
        public virtual ICollection<UserFeatureState> UserFeatureState { get; set; }
        public virtual ICollection<UserIncome> UserIncome { get; set; }
        public virtual ICollection<UserNotification> UserNotification { get; set; }
    }
}
