using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class Store
    {
        public int UserStoreId { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public float Amount { get; set; }

        public StoreType StoreNavigation { get; set; }
        public User User { get; set; }
    }
}
