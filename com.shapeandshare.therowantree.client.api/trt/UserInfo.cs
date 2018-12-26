using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.trt
{
    public partial class UserInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTimeOffset CreateTime { get; set; }

        public User User { get; set; }
    }
}
