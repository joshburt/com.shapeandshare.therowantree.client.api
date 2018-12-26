using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.trt
{
    public partial class User
    {
        public User()
        {
            UserNotification = new HashSet<UserNotification>();
        }

        public int UserId { get; set; }
        public string Guid { get; set; }
        public byte? Active { get; set; }
        public int Population { get; set; }

        public UserInfo UserInfo { get; set; }
        public ICollection<UserNotification> UserNotification { get; set; }
    }
}
